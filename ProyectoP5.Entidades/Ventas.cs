using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoP5.Entidades
{
    class Ventas
    {
        public DateTime Fecha { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<Producto> Productos { get; set; }
        public int Cantidad { get; set; }
        public int Total { get; set; }

        public Ventas(DateTime fecha, List<Cliente> clientes, List<Producto> productos, int cantidad, int total)
        {
            Fecha = fecha;
            Clientes = clientes;
            Productos = productos;
            Cantidad = cantidad;
            Total = total;
        }
    }
}
