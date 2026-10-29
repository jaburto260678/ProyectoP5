
namespace ProyectoP5.Entidades
{
    public class VentaClienteOpcion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public override string ToString()
        {
            return Nombre;
        }
    }
}
