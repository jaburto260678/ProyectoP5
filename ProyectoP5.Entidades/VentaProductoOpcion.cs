
namespace ProyectoP5.Entidades
{
    public class VentaProductoOpcion
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public override string ToString()
        {
            return $"{Codigo} - {Nombre} | ₡{Precio:N2} | Stock: {Stock}";
        }
    }
}
