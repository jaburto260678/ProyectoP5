
namespace ProyectoP5.Entidades
{
    /// <summary>
    /// Representa un producto registrado
    /// en el detalle de una venta.
    /// </summary>
    public class VentaDetalleHistorialItem
    {
        public int Id { get; set; }

        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}
