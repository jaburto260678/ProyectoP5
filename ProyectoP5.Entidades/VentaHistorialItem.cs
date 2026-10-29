
using System;

namespace ProyectoP5.Entidades
{
    /// <summary>
    /// Representa la información general de una venta
    /// registrada en el sistema.
    /// </summary>
    public class VentaHistorialItem
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public int ClienteId { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }
}
