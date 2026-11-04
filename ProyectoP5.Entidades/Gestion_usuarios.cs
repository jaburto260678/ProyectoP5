
using ProyectoP5.Datos;
using MySql.Data.MySqlClient;


namespace ProyectoP5.Entidades
{
    public class Gestion_usuarios
    {
        public static int Agregar(Cliente cliente)
        {
            using MySqlConnection conexion = ConexionBD.CrearConexion();
            using MySqlCommand comando = new MySqlCommand(
                "INSERT INTO clientes (nombre, telefono, correo, activo) " +
                "VALUES (@nombre, @telefono, @correo, @activo)",
                conexion);

            comando.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = cliente.Nombre;
            comando.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = cliente.Telefono;
            comando.Parameters.Add("@correo", MySqlDbType.VarChar).Value = cliente.Correo;
            comando.Parameters.Add("@activo", MySqlDbType.Int32).Value = 1;

            return comando.ExecuteNonQuery();
        }
        public static List<Cliente> Obtenertodos()
        {
            List<Cliente> lista = new List<Cliente>();

            String sql = "SELECT id, nombre, telefono, correo, activo FROM clientes WHERE activo = 1";

            MySqlConnection conexion = ConexionBD.CrearConexion();
            MySqlCommand comando = new MySqlCommand(sql, conexion);

            MySqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Cliente cliente = new Cliente();
                cliente.Id = reader.GetInt32(0);
                cliente.Nombre = reader.GetString(1);
                cliente.Telefono = reader.GetString(2);
                cliente.Correo = reader.GetString(3);

                lista.Add(cliente);
            }
            return lista;
        }

        public static List<Cliente> Buscar(string texto)
        {
            List<Cliente> lista = new List<Cliente>();

            string sql = @"SELECT id, nombre, telefono, correo, activo
                   FROM clientes
                   WHERE activo = 1
                     AND (CAST(id AS CHAR) = @exacto
                      OR nombre   LIKE @parcial
                      OR telefono LIKE @parcial
                      OR correo   LIKE @parcial)";

            using (MySqlConnection conexion = ConexionBD.CrearConexion())
            using (MySqlCommand comando = new MySqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@exacto", texto);
                comando.Parameters.AddWithValue("@parcial", "%" + texto + "%");

                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente();
                        cliente.Id = reader.GetInt32(0);
                        cliente.Nombre = reader.GetString(1);
                        cliente.Telefono = reader.GetString(2);
                        cliente.Correo = reader.GetString(3);

                        lista.Add(cliente);
                    }
                }
            }

            return lista;
        }
        public static Cliente ObtenerCliente(int Id)
        {
            Cliente cliente = new Cliente();
            MySqlConnection conexion = ConexionBD.CrearConexion(); 

            MySqlCommand comando = new MySqlCommand(String.Format("SELECT id, nombre, telefono, correo, activo  FROM clientes WHERE id={0}", Id), conexion);
            MySqlDataReader reader = comando.ExecuteReader();
            while(reader.Read())
            {
                cliente.Id = reader.GetInt32(0);
                cliente.Nombre = reader.GetString(1);
                cliente.Telefono = reader.GetString(2);
                cliente.Correo = reader.GetString(3);

            }
            conexion.Close();
            return cliente;

        }
        public static int Actualizar(Cliente cliente)
        {
            int resultado = 0;

            MySqlConnection conexion = ConexionBD.CrearConexion();

             MySqlCommand comando = new MySqlCommand(String.Format("UPDATE clientes set nombre = '{0}', telefono = '{1}', correo = '{2}' WHERE id = {3}", cliente.Nombre, cliente.Telefono, cliente.Correo, cliente.Id), conexion);
            resultado = comando.ExecuteNonQuery();
            conexion.Close();
            return resultado;

            }


        public static int Eliminar(Cliente cliente)
        {
            using MySqlConnection conexion = ConexionBD.CrearConexion();
            using MySqlCommand comando = new MySqlCommand(
                "UPDATE clientes SET activo = 0 WHERE id = @id", conexion);

            comando.Parameters.Add("@id", MySqlDbType.Int32).Value = cliente.Id;

            return comando.ExecuteNonQuery();

        }
    }
 }