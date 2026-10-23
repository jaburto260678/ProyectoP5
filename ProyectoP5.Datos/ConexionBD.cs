using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace ProyectoP5.Datos
{
    public static class ConexionBD
    {
        /// Crear Comando Conexión
        private const string Conexion =
        "Server=localhost;" +
        "Port=3306;" +
        "Database=sistema_ventas;" +
        "User ID=;" +
        "Password=;";

        /// Crear Conexión
        public static MySqlConnection CrearConexion()
        {
            return new MySqlConnection(Conexion);
        }
    }
}
