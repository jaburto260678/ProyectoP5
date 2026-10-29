
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
        /// Consulta los clientes activos registrados.
        /// </summary>
        public List<VentaClienteOpcion> ObtenerClientes()
        {
            return ventaDAO.ObtenerClientes();
        }

        /// <summary>
        /// Consulta los productos disponibles para vender.
        /// </summary>
        public List<VentaProductoOpcion> ObtenerProductos()
        {
            return ventaDAO.ObtenerProductos();
        }

        /// <summary>
        /// Calcula el importe total de los productos.
        /// </summary>
        public decimal CalcularTotal(
            List<DetalleVenta> detalles)
        {
            ValidarDetalles(detalles);

            return detalles.Sum(d => d.Subtotal);
        }

        /// <summary>
        /// Comprueba que la venta tenga información válida.
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
        /// Comprueba los productos, cantidades y precios.
        /// </summary>
        private void ValidarDetalles(
            List<DetalleVenta> detalles)
        {
            if (detalles == null ||
                detalles.Count == 0)
            {
                throw new ArgumentException(
                    "La venta no contiene productos.");
            }

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

                try
                {
                    // Comprobar que no haya desbordamiento
                    // en el cálculo del subtotal.
                    decimal subtotal = checked(
                        detalle.Cantidad * detalle.PrecioUnitario);

                    if (subtotal < 0)
                    {
                        throw new ArgumentException(
                            "El subtotal no puede ser negativo.");
                    }
                }
                catch (OverflowException)
                {
                    throw new ArgumentException(
                        "El importe del producto supera " +
                        "el máximo permitido.");
                }
            }

            // Comprobar que el total pueda calcularse.
            try
            {
                decimal total = 0;

                foreach (DetalleVenta detalle in detalles)
                {
                    total = checked(
                        total +
                        detalle.Cantidad *
                        detalle.PrecioUnitario);
                }
            }
            catch (OverflowException)
            {
                throw new ArgumentException(
                    "El importe total supera " +
                    "el máximo permitido.");
            }

            // Evitar cantidades acumuladas demasiado grandes.
            var grupos = detalles.GroupBy(
                detalle => detalle.ProductoId);

            foreach (var grupo in grupos)
            {
                long cantidadTotal = grupo.Sum(
                    detalle => (long)detalle.Cantidad);

                if (cantidadTotal > int.MaxValue)
                {
                    throw new ArgumentException(
                        "La cantidad total de un producto " +
                        "supera el máximo permitido.");
                }

                if (grupo.Select(
                    d => d.PrecioUnitario).Distinct().Count() > 1)
                {
                    throw new ArgumentException(
                        "Un producto tiene precios diferentes " +
                        "en la misma venta.");
                }
            }
        }

        /// <summary>
        /// Valida los datos y registra la venta en MySQL.
        /// </summary>
        public int RegistrarVenta(VentaRegistro venta)
        {
            ValidarVenta(venta);

            // El DAO revisa el cliente, stock y precio real.
            // También controla la transacción completa.
            return ventaDAO.RegistrarVenta(venta);
        }
    }
}
