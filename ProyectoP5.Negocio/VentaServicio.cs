
using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoP5.Datos;
using ProyectoP5.Entidades;

namespace ProyectoP5.Negocio
{
    /// <summary>
    /// Contiene las validaciones y operaciones
    /// principales del módulo de Ventas.
    /// </summary>
    public class VentaServicio
    {
        private readonly VentaDAO ventaDAO;

        /// <summary>
        /// Inicializa el servicio de ventas.
        /// </summary>
        public VentaServicio()
        {
            ventaDAO = new VentaDAO();
        }

        /// <summary>
        /// Obtiene los clientes activos para
        /// mostrarlos en el formulario.
        /// </summary>
        public List<VentaClienteOpcion> ObtenerClientes()
        {
            return ventaDAO.ObtenerClientes();
        }

        /// <summary>
        /// Obtiene los productos disponibles
        /// desde la base de datos.
        /// </summary>
        public List<VentaProductoOpcion> ObtenerProductos()
        {
            return ventaDAO.ObtenerProductos();
        }

        /// <summary>
        /// Calcula el total de los productos.
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
        /// Comprueba que una venta tenga
        /// la información necesaria.
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
        /// Revisa los productos y cantidades
        /// antes de registrar la venta.
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
        /// Valida y registra una venta en MySQL.
        /// </summary>
        public int RegistrarVenta(VentaRegistro venta)
        {
            ValidarVenta(venta);

            // El DAO comprueba los precios reales,
            // las existencias y registra los datos.
            return ventaDAO.RegistrarVenta(venta);
        }
    }
}
