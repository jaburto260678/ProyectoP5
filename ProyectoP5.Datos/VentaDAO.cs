
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;
using ProyectoP5.Entidades;

namespace ProyectoP5.Datos
{
    /// <summary>
    /// Se encarga de registrar las ventas en MySQL.
    /// </summary>
    public class VentaDAO
    {
        /// <summary>
        /// Guarda una venta, sus detalles y actualiza el inventario.
        /// Devuelve el ID de la venta registrada.
        /// </summary>
        public int RegistrarVenta(VentaRegistro venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));

            if (venta.ClienteId <= 0)
                throw new ArgumentException("Seleccione un cliente.");

            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new ArgumentException("Agregue productos a la venta.");

            // Agrupar productos repetidos.
            var detalles = venta.Detalles
                .GroupBy(d => d.ProductoId)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    Cantidad = g.Sum(d => (long)d.Cantidad)
                })
                .OrderBy(d => d.ProductoId)
                .ToList();

            foreach (var detalle in venta.Detalles)
            {
                if (detalle.ProductoId <= 0 || detalle.Cantidad <= 0)
                    throw new ArgumentException(
                        "Los productos y cantidades deben ser válidos.");
            }

            using var conexion = VentaConexion.CrearConexion();
            conexion.Open();

            using var transaccion = conexion.BeginTransaction();

            try
            {
                // Comprobar que el cliente está activo.
                const string sqlCliente = @"
                    SELECT COUNT(*)
                    FROM clientes
                    WHERE id = @id AND activo = 1";

                using (var comando = new MySqlCommand(
                    sqlCliente, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue("@id", venta.ClienteId);

                    long existe = Convert.ToInt64(comando.ExecuteScalar());

                    if (existe == 0)
                        throw new InvalidOperationException(
                            "El cliente no existe o está inactivo.");
                }

                var productos = new List<DetalleVenta>();

                // Consultar precio y bloquear stock durante la venta.
                foreach (var detalle in detalles)
                {
                    if (detalle.Cantidad > int.MaxValue)
                        throw new InvalidOperationException(
                            "La cantidad solicitada es demasiado grande.");

                    const string sqlProducto = @"
                        SELECT nombre, precio, stock
                        FROM productos
                        WHERE id = @id AND activo = 1
                        FOR UPDATE";

                    using var comando = new MySqlCommand(
                        sqlProducto, conexion, transaccion);

                    comando.Parameters.AddWithValue(
                        "@id", detalle.ProductoId);

                    using var lector = comando.ExecuteReader();

                    if (!lector.Read())
                        throw new InvalidOperationException(
                            $"Producto {detalle.ProductoId} no disponible.");

                    string nombre = lector.GetString(0);
                    decimal precio = lector.GetDecimal(1);
                    int stock = lector.GetInt32(2);

                    if (stock < detalle.Cantidad)
                        throw new InvalidOperationException(
                            $"Stock insuficiente para {nombre}.");

                    productos.Add(new DetalleVenta
                    {
                        ProductoId = detalle.ProductoId,
                        NombreProducto = nombre,
                        Cantidad = (int)detalle.Cantidad,
                        PrecioUnitario = precio
                    });
                }

                decimal total = productos.Sum(p => p.Subtotal);

                // Registrar cabecera de venta.
                const string sqlVenta = @"
                    INSERT INTO ventas (fecha, cliente_id, total)
                    VALUES (@fecha, @cliente, @total)";

                int ventaId;

                using (var comando = new MySqlCommand(
                    sqlVenta, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue(
                        "@fecha", DateTime.Now);
                    comando.Parameters.AddWithValue(
                        "@cliente", venta.ClienteId);
                    comando.Parameters.AddWithValue("@total", total);

                    comando.ExecuteNonQuery();
                    ventaId = checked((int)comando.LastInsertedId);
                }

                // Guardar cada producto y descontar existencias.
                foreach (var producto in productos)
                {
                    const string sqlDetalle = @"
                        INSERT INTO detalle_venta
                        (venta_id, producto_id, cantidad,
                         precio_unitario, subtotal)
                        VALUES
                        (@venta, @producto, @cantidad,
                         @precio, @subtotal)";

                    using (var comando = new MySqlCommand(
                        sqlDetalle, conexion, transaccion))
                    {
                        comando.Parameters.AddWithValue(
                            "@venta", ventaId);
                        comando.Parameters.AddWithValue(
                            "@producto", producto.ProductoId);
                        comando.Parameters.AddWithValue(
                            "@cantidad", producto.Cantidad);
                        comando.Parameters.AddWithValue(
                            "@precio", producto.PrecioUnitario);
                        comando.Parameters.AddWithValue(
                            "@subtotal", producto.Subtotal);

                        comando.ExecuteNonQuery();
                    }

                    const string sqlStock = @"
                        UPDATE productos
                        SET stock = stock - @cantidad
                        WHERE id = @producto
                        AND stock >= @cantidad";

                    using (var comando = new MySqlCommand(
                        sqlStock, conexion, transaccion))
                    {
                        comando.Parameters.AddWithValue(
                            "@cantidad", producto.Cantidad);
                        comando.Parameters.AddWithValue(
                            "@producto", producto.ProductoId);

                        int filas = comando.ExecuteNonQuery();

                        if (filas != 1)
                            throw new InvalidOperationException(
                                "No se pudo actualizar el inventario.");
                    }
                }

                // Confirmar todas las operaciones.
                transaccion.Commit();
                return ventaId;
            }
            catch
            {
                // Cancelar los cambios si ocurre un error.
                transaccion.Rollback();
                throw;
            }
        }
    }
}
