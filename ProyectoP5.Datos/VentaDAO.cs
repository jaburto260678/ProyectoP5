
using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using ProyectoP5.Entidades;

namespace ProyectoP5.Datos
{
    /// <summary>
    /// Maneja las operaciones de ventas en la base
    /// de datos MySQL.
    /// </summary>
    public class VentaDAO
    {
        /// <summary>
        /// Obtiene los clientes activos registrados.
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

            using var comando = new MySqlCommand(
                sql, conexion);

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
        /// Obtiene los productos activos que tienen
        /// existencias disponibles.
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

            using var comando = new MySqlCommand(
                sql, conexion);

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
        /// Registra una venta, guarda sus detalles
        /// y descuenta las existencias del inventario.
        /// Devuelve el ID de la venta registrada.
        /// </summary>
        public int RegistrarVenta(VentaRegistro venta)
        {
            if (venta == null)
            {
                throw new ArgumentNullException(
                    nameof(venta));
            }

            if (venta.ClienteId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar un cliente válido.");
            }

            if (venta.Detalles == null ||
                venta.Detalles.Count == 0)
            {
                throw new ArgumentException(
                    "Debe agregar al menos un producto.");
            }

            // Validar todos los detalles antes de guardar.
            foreach (var detalle in venta.Detalles)
            {
                if (detalle == null ||
                    detalle.ProductoId <= 0 ||
                    detalle.Cantidad <= 0)
                {
                    throw new ArgumentException(
                        "La venta contiene productos " +
                        "o cantidades no válidas.");
                }
            }

            // Agrupar productos repetidos.
            var detallesAgrupados = venta.Detalles
                .GroupBy(d => d.ProductoId)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    Cantidad = g.Sum(d => (long)d.Cantidad)
                })
                .OrderBy(d => d.ProductoId)
                .ToList();

            using var conexion =
                VentaConexion.CrearConexion();

            conexion.Open();

            using var transaccion =
                conexion.BeginTransaction();

            try
            {
                // Verificar que el cliente exista y esté activo.
                const string sqlCliente = @"
                    SELECT COUNT(*)
                    FROM clientes
                    WHERE id = @id AND activo = 1";

                using (var comando = new MySqlCommand(
                    sqlCliente, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue(
                        "@id", venta.ClienteId);

                    long cantidadClientes =
                        Convert.ToInt64(comando.ExecuteScalar());

                    if (cantidadClientes == 0)
                    {
                        throw new InvalidOperationException(
                            "El cliente no existe o está inactivo.");
                    }
                }

                var productosVenta =
                    new List<DetalleVenta>();

                // Consultar el precio real y bloquear
                // cada producto durante la transacción.
                foreach (var detalle in detallesAgrupados)
                {
                    if (detalle.Cantidad > int.MaxValue)
                    {
                        throw new InvalidOperationException(
                            "La cantidad solicitada es demasiado grande.");
                    }

                    const string sqlProducto = @"
                        SELECT nombre, precio, stock
                        FROM productos
                        WHERE id = @id AND activo = 1
                        FOR UPDATE";

                    using var comando = new MySqlCommand(
                        sqlProducto, conexion, transaccion);

                    comando.Parameters.AddWithValue(
                        "@id", detalle.ProductoId);

                    string nombre;
                    decimal precio;
                    int stock;

                    using (var lector = comando.ExecuteReader())
                    {
                        if (!lector.Read())
                        {
                            throw new InvalidOperationException(
                                $"El producto {detalle.ProductoId} " +
                                "no está disponible.");
                        }

                        nombre = lector.GetString(0);
                        precio = lector.GetDecimal(1);
                        stock = lector.GetInt32(2);
                    }

                    if (stock < detalle.Cantidad)
                    {
                        throw new InvalidOperationException(
                            $"No hay suficiente stock de {nombre}. " +
                            $"Disponible: {stock}.");
                    }

                    productosVenta.Add(new DetalleVenta
                    {
                        ProductoId = detalle.ProductoId,
                        NombreProducto = nombre,
                        Cantidad = (int)detalle.Cantidad,
                        PrecioUnitario = precio
                    });
                }

                // Calcular el importe con precios de MySQL.
                decimal total = productosVenta
                    .Sum(p => p.Subtotal);

                // Guardar la cabecera.
                const string sqlVenta = @"
                    INSERT INTO ventas
                    (fecha, cliente_id, total)
                    VALUES
                    (@fecha, @cliente, @total)";

                int ventaId;

                using (var comando = new MySqlCommand(
                    sqlVenta, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue(
                        "@fecha", DateTime.Now);

                    comando.Parameters.AddWithValue(
                        "@cliente", venta.ClienteId);

                    comando.Parameters.AddWithValue(
                        "@total", total);

                    comando.ExecuteNonQuery();

                    ventaId = checked(
                        (int)comando.LastInsertedId);
                }

                // Guardar los detalles de la venta
                // y actualizar el inventario.
                foreach (var producto in productosVenta)
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

                        int filasAfectadas =
                            comando.ExecuteNonQuery();

                        if (filasAfectadas != 1)
                        {
                            throw new InvalidOperationException(
                                "No se pudo actualizar el stock " +
                                "del producto " +
                                producto.NombreProducto);
                        }
                    }
                }

                // Confirmar todos los cambios.
                transaccion.Commit();

                return ventaId;
            }
            catch
            {
                // Cancelar los cambios si ocurre algún error.
                transaccion.Rollback();
                throw;
            }
        }
    }
}
