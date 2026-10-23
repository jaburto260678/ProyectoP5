using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoP5.Entidades
{
    class Cliente
    {
        // Valores
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public Cliente(int id, string nombre, string telefono, string correo) 
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
            Correo = correo;
        }
    }
}
