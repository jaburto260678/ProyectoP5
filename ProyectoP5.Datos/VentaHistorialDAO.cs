
using System;
using System.Collections.Generic;
using MySqlConnector;
using ProyectoP5.Entidades;

namespace ProyectoP5.Datos
{
    /// <summary>
    /// Realiza las consultas del historial de ventas
    /// en la base de datos MySQL.
    /// </summary>
    public class VentaHistorialDAO
    {
        /// <summary>
        /// Obtiene las ventas registradas
        /// dentro de un rango de fechas.
        /// </summary>
        public List<VentaHistorialItem> ObtenerVentas(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            var ventas = new List<VentaHistorialItem>();

            using var conexion =
                VentaConexion.CrearConexion();

            conexion.Open();

            const string sql = @"
                SELECT
                    v.id,
                    v.fecha,
                    v.cliente_id,
                    c.nombre,
                    v.total
                FROM ventas v
                INNER JOIN clientes c
                    ON v.cliente_id = c.id
                WHERE v.fecha >= @inicio
                  AND v.fecha < @fin
                ORDER BY v.fecha DESC, v.id DESC";

            using var comando =
                new MySqlCommand(sql, conexion);

            comando.Parameters.AddWithValue(
                "@inicio", fechaInicio.Date);

            comando.Parameters.AddWithValue(
                "@fin", fechaFin.Date.AddDays(1));

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var venta = new VentaHistorialItem
                {
                    Id = lector.GetInt32(0),
                    Fecha = lector.GetDateTime(1),
                    ClienteId = lector.GetInt32(2),
                    Cliente = lector.GetString(3),
                    Total = lector.GetDecimal(4)
                };

                ventas.Add(venta);
            }

            return ventas;
        }

        /// <summary>
        /// Obtiene todos los productos asociados
        /// a una venta específica.
        /// </summary>
        public List<VentaDetalleHistorialItem> ObtenerDetalle(
            int ventaId)
        {
            var detalles =
                new List<VentaDetalleHistorialItem>();

            using var conexion =
                VentaConexion.CrearConexion();

            conexion.Open();

            const string sql = @"
                SELECT
                    dv.id,
                    dv.venta_id,
                    dv.producto_id,
                    p.codigo,
                    p.nombre,
                    dv.cantidad,
                    dv.precio_unitario,
                    dv.subtotal
                FROM detalle_venta dv
                INNER JOIN productos p
                    ON dv.producto_id = p.id
                WHERE dv.venta_id = @ventaId
                ORDER BY dv.id";

            using var comando =
                new MySqlCommand(sql, conexion);

            comando.Parameters.AddWithValue(
                "@ventaId", ventaId);

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var detalle = new VentaDetalleHistorialItem
                {
                    Id = lector.GetInt32(0),
                    VentaId = lector.GetInt32(1),
                    ProductoId = lector.GetInt32(2),
                    Codigo = lector.GetString(3),
                    Producto = lector.GetString(4),
                    Cantidad = lector.GetInt32(5),
                    PrecioUnitario = lector.GetDecimal(6),
                    Subtotal = lector.GetDecimal(7)
                };

                detalles.Add(detalle);
            }

            return detalles;
        }
    }
}
