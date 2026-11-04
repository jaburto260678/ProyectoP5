using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace ProyectoP5.Datos
{
    public class ConexionBD
    {
        /// Crear Conexión
        public static MySqlConnection CrearConexion()
        {
            MySqlConnection conectar = new MySqlConnection("Server=localhost;Port=3306;Database=sistema_ventas;User ID=root;Password=;");
            conectar.Open();
            return conectar;
        }
    }
}
