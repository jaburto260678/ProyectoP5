namespace ProyectoP5.Entidades
{
    /// <summary>
    /// Representa un producto incluido en una venta.
    /// </summary>
    public class DetalleVenta
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal
        {
            get { return Cantidad * PrecioUnitario; }
        }
    }
}