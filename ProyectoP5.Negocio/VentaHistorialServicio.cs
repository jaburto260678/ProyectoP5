
using System;
using System.Collections.Generic;
using ProyectoP5.Datos;
using ProyectoP5.Entidades;

namespace ProyectoP5.Negocio
{
    /// <summary>
    /// Contiene las reglas de negocio
    /// para consultar el historial de ventas.
    /// </summary>
    public class VentaHistorialServicio
    {
        private readonly VentaHistorialDAO historialDAO;

        public VentaHistorialServicio()
        {
            historialDAO = new VentaHistorialDAO();
        }

        /// <summary>
        /// Busca las ventas registradas
        /// dentro de un rango de fechas.
        /// </summary>
        public List<VentaHistorialItem> ConsultarVentas(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            if (fechaInicio.Date > fechaFin.Date)
            {
                throw new ArgumentException(
                    "La fecha inicial no puede ser " +
                    "mayor que la fecha final.");
            }

            if (fechaFin.Date >= DateTime.MaxValue.Date)
            {
                throw new ArgumentException(
                    "Seleccione una fecha final válida.");
            }

            return historialDAO.ObtenerVentas(
                fechaInicio, fechaFin);
        }

        /// <summary>
        /// Consulta los productos registrados
        /// en una venta específica.
        /// </summary>
        public List<VentaDetalleHistorialItem> ConsultarDetalle(
            int ventaId)
        {
            if (ventaId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar una venta válida.");
            }

            return historialDAO.ObtenerDetalle(ventaId);
        }
    }
}
