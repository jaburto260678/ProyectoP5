
using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoP5.Datos;
using ProyectoP5.Entidades;

namespace ProyectoP5.Negocio
{
    /// <summary>
    /// Contiene las reglas de negocio del módulo de Ventas.
    /// </summary>
    public class VentaServicio
    {
        private readonly VentaDAO ventaDAO;

        public VentaServicio()
        {
            ventaDAO = new VentaDAO();
        }

        /// <summary>
        /// Calcula el total de una lista de productos.
        /// </summary>
        public decimal CalcularTotal(
            List<DetalleVenta> detalles)
        {
            if (detalles == null)
            {
                throw new ArgumentException(
                    "La lista de productos no puede ser nula.");
            }

            ValidarDetalles(detalles);

            return detalles.Sum(d => d.Subtotal);
        }

        /// <summary>
        /// Verifica que la venta tenga datos válidos.
        /// </summary>
        public void ValidarVenta(VentaRegistro venta)
        {
            if (venta == null)
            {
                throw new ArgumentException(
                    "No se recibió información de la venta.");
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

            ValidarDetalles(venta.Detalles);
        }

        /// <summary>
        /// Valida los productos y las cantidades ingresadas.
        /// </summary>
        private void ValidarDetalles(
            List<DetalleVenta> detalles)
        {
            foreach (DetalleVenta detalle in detalles)
            {
                if (detalle == null)
                {
                    throw new ArgumentException(
                        "La venta contiene un detalle inválido.");
                }

                if (detalle.ProductoId <= 0)
                {
                    throw new ArgumentException(
                        "Debe seleccionar un producto válido.");
                }

                if (detalle.Cantidad <= 0)
                {
                    throw new ArgumentException(
                        "La cantidad debe ser mayor que cero.");
                }

                if (detalle.PrecioUnitario < 0)
                {
                    throw new ArgumentException(
                        "El precio no puede ser negativo.");
                }
            }
        }

        /// <summary>
        /// Valida y registra una venta en la base de datos.
        /// </summary>
        public int RegistrarVenta(VentaRegistro venta)
        {
            ValidarVenta(venta);

            // El DAO consulta los precios reales en MySQL
            // y verifica el stock antes de guardar.
            return ventaDAO.RegistrarVenta(venta);
        }
    }
}
