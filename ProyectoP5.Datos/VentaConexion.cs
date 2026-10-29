
using System;
using MySqlConnector;

namespace ProyectoP5.Datos
{
    /// <summary>
    /// Maneja la conexión de MySQL para el módulo de Ventas.
    /// </summary>
    public static class VentaConexion
    {
        /// <summary>
        /// Crea una conexión utilizando la configuración
        /// almacenada localmente en Windows.
        /// </summary>
        public static MySqlConnection CrearConexion()
        {
            string? cadena = Environment.GetEnvironmentVariable(
                "SISTEMA_VENTAS_CONNECTION");

            if (string.IsNullOrWhiteSpace(cadena))
            {
                throw new InvalidOperationException(
                    "No se encontró la configuración de MySQL. " +
                    "Configure SISTEMA_VENTAS_CONNECTION " +
                    "en las variables de entorno de Windows.");
            }

            return new MySqlConnection(cadena);
        }
    }
}
