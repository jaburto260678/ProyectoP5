
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoP5.Entidades
{
    /// <summary>
    /// Representa una venta con su cliente y productos.
    /// </summary>
    public class VentaRegistro
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }

        public List<DetalleVenta> Detalles { get; set; }
            = new List<DetalleVenta>();

        public decimal Total
        {
            get { return Detalles.Sum(d => d.Subtotal); }
        }
    }
}
