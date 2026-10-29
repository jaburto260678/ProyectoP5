
using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using ProyectoP5.Entidades;

namespace ProyectoP5.Datos
{
    /// <summary>
    /// Maneja las consultas y el registro de ventas en MySQL.
    /// </summary>
    public class VentaDAO
    {
        /// <summary>
        /// Obtiene los clientes activos de la base de datos.
        /// </summary>
        public List<VentaClienteOpcion> ObtenerClientes()
        {
            var clientes = new List<VentaClienteOpcion>();

            using var conexion = VentaConexion.CrearConexion();
            conexion.Open();

            const string sql = @"
                SELECT id, nombre
                FROM clientes
                WHERE activo = 1
                ORDER BY nombre";

            using var comando = new MySqlCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                clientes.Add(new VentaClienteOpcion
                {
                    Id = lector.GetInt32(0),
                    Nombre = lector.GetString(1)
                });
            }

            return clientes;
        }

        /// <summary>
        /// Obtiene los productos activos con stock disponible.
        /// </summary>
        public List<VentaProductoOpcion> ObtenerProductos()
        {
            var productos = new List<VentaProductoOpcion>();

            using var conexion = VentaConexion.CrearConexion();
            conexion.Open();

            const string sql = @"
                SELECT id, codigo, nombre, precio, stock
                FROM productos
                WHERE activo = 1 AND stock > 0
                ORDER BY nombre";

            using var comando = new MySqlCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                productos.Add(new VentaProductoOpcion
                {
                    Id = lector.GetInt32(0),
                    Codigo = lector.GetString(1),
                    Nombre = lector.GetString(2),
                    Precio = lector.GetDecimal(3),
                    Stock = lector.GetInt32(4)
                });
            }

            return productos;
        }

        /// <summary>
        /// Registra una venta y actualiza las existencias.
        /// Devuelve el identificador de la venta registrada.
        /// </summary>
        public int RegistrarVenta(VentaRegistro venta)
        {
            if (venta == null)
            {
                throw new ArgumentNullException(nameof(venta));
            }

            if (venta.ClienteId <= 0)
            {
                throw new ArgumentException(
                    "Seleccione un cliente válido.");
            }

            if (venta.Detalles == null ||
                venta.Detalles.Count == 0)
            {
                throw new ArgumentException(
                    "Agregue al menos un producto.");
            }

            // Validar los detalles antes de conectar con MySQL.
            foreach (DetalleVenta detalle in venta.Detalles)
            {
                if (detalle == null ||
                    detalle.ProductoId <= 0 ||
                    detalle.Cantidad <= 0)
                {
                    throw new ArgumentException(
                        "Existen productos o cantidades incorrectas.");
                }

                if (detalle.PrecioUnitario < 0)
                {
                    throw new ArgumentException(
                        "El precio del producto no puede ser negativo.");
                }
            }

            // Agrupar productos repetidos y sumar cantidades.
            var detallesAgrupados = venta.Detalles
                .GroupBy(d => d.ProductoId)
                .Select(grupo => new
                {
                    ProductoId = grupo.Key,
                    Cantidad = grupo.Sum(d => (long)d.Cantidad),
                    Precios = grupo.Select(
                        d => d.PrecioUnitario).Distinct().ToList()
                })
                .OrderBy(d => d.ProductoId)
                .ToList();

            using var conexion = VentaConexion.CrearConexion();
            conexion.Open();

            using var transaccion = conexion.BeginTransaction();

            try
            {
                // Verificar que el cliente exista y esté activo.
                const string sqlCliente = @"
                    SELECT COUNT(*)
                    FROM clientes
                    WHERE id = @clienteId AND activo = 1";

                using (var comando = new MySqlCommand(
                    sqlCliente, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue(
                        "@clienteId", venta.ClienteId);

                    long cantidadClientes =
                        Convert.ToInt64(comando.ExecuteScalar());

                    if (cantidadClientes == 0)
                    {
                        throw new InvalidOperationException(
                            "El cliente no existe o está inactivo.");
                    }
                }

                var productosVenta = new List<DetalleVenta>();

                // Bloquear los productos mientras se registra
                // la venta para evitar inconsistencias de stock.
                foreach (var detalle in detallesAgrupados)
                {
                    if (detalle.Cantidad > int.MaxValue)
                    {
                        throw new InvalidOperationException(
                            "La cantidad solicitada es demasiado grande.");
                    }

                    if (detalle.Precios.Count != 1)
                    {
                        throw new InvalidOperationException(
                            "Hay precios diferentes para el mismo producto. " +
                            "Actualice los datos y vuelva a intentarlo.");
                    }

                    const string sqlProducto = @"
                        SELECT nombre, precio, stock
                        FROM productos
                        WHERE id = @productoId AND activo = 1
                        FOR UPDATE";

                    using var comando = new MySqlCommand(
                        sqlProducto, conexion, transaccion);

                    comando.Parameters.AddWithValue(
                        "@productoId", detalle.ProductoId);

                    string nombre;
                    decimal precioActual;
                    int stockActual;

                    using (var lector = comando.ExecuteReader())
                    {
                        if (!lector.Read())
                        {
                            throw new InvalidOperationException(
                                $"El producto {detalle.ProductoId} " +
                                "no existe o está inactivo.");
                        }

                        nombre = lector.GetString(0);
                        precioActual = lector.GetDecimal(1);
                        stockActual = lector.GetInt32(2);
                    }

                    // Detectar cambios de precio.
                    if (precioActual != detalle.Precios[0])
                    {
                        throw new InvalidOperationException(
                            $"El precio de {nombre} cambió a " +
                            $"₡{precioActual:N2}. " +
                            "Actualice los productos y vuelva " +
                            "a preparar la venta.");
                    }

                    // Comprobar las existencias.
                    if (stockActual < detalle.Cantidad)
                    {
                        throw new InvalidOperationException(
                            $"Stock insuficiente para {nombre}. " +
                            $"Disponible: {stockActual}.");
                    }

                    productosVenta.Add(new DetalleVenta
                    {
                        ProductoId = detalle.ProductoId,
                        NombreProducto = nombre,
                        Cantidad = (int)detalle.Cantidad,
                        PrecioUnitario = precioActual
                    });
                }

                // Calcular el total utilizando precios
                // comprobados directamente en MySQL.
                decimal total = productosVenta.Sum(
                    producto => producto.Subtotal);

                // Guardar la cabecera de la venta.
                const string sqlVenta = @"
                    INSERT INTO ventas
                        (fecha, cliente_id, total)
                    VALUES
                        (@fecha, @clienteId, @total)";

                int ventaId;

                using (var comando = new MySqlCommand(
                    sqlVenta, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue(
                        "@fecha", DateTime.Now);

                    comando.Parameters.AddWithValue(
                        "@clienteId", venta.ClienteId);

                    comando.Parameters.AddWithValue(
                        "@total", total);

                    comando.ExecuteNonQuery();

                    ventaId = checked(
                        (int)comando.LastInsertedId);
                }

                // Guardar los productos vendidos.
                foreach (DetalleVenta producto in productosVenta)
                {
                    const string sqlDetalle = @"
                        INSERT INTO detalle_venta
                            (venta_id, producto_id, cantidad,
                             precio_unitario, subtotal)
                        VALUES
                            (@ventaId, @productoId, @cantidad,
                             @precio, @subtotal)";

                    using (var comando = new MySqlCommand(
                        sqlDetalle, conexion, transaccion))
                    {
                        comando.Parameters.AddWithValue(
                            "@ventaId", ventaId);

                        comando.Parameters.AddWithValue(
                            "@productoId", producto.ProductoId);

                        comando.Parameters.AddWithValue(
                            "@cantidad", producto.Cantidad);

                        comando.Parameters.AddWithValue(
                            "@precio", producto.PrecioUnitario);

                        comando.Parameters.AddWithValue(
                            "@subtotal", producto.Subtotal);

                        comando.ExecuteNonQuery();
                    }

                    // Descontar el inventario únicamente
                    // si existe stock suficiente.
                    const string sqlActualizarStock = @"
                        UPDATE productos
                        SET stock = stock - @cantidad
                        WHERE id = @productoId
                          AND activo = 1
                          AND stock >= @cantidad";

                    using (var comando = new MySqlCommand(
                        sqlActualizarStock, conexion, transaccion))
                    {
                        comando.Parameters.AddWithValue(
                            "@cantidad", producto.Cantidad);

                        comando.Parameters.AddWithValue(
                            "@productoId", producto.ProductoId);

                        int filasAfectadas =
                            comando.ExecuteNonQuery();

                        if (filasAfectadas != 1)
                        {
                            throw new InvalidOperationException(
                                "No se pudo actualizar el inventario " +
                                $"de {producto.NombreProducto}.");
                        }
                    }
                }

                // Confirmar la venta y sus detalles.
                transaccion.Commit();

                return ventaId;
            }
            catch
            {
                // Deshacer todos los cambios
                // si alguna operación falla.
                transaccion.Rollback();
                throw;
            }
        }
    }
}
