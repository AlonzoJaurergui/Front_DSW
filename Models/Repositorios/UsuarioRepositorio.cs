using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;

namespace PizzaNicola_AspNetCore.Models.Repositorios
{
    public class UsuarioRepositorio : IUsuario
    {
        private string cadena;

        public UsuarioRepositorio()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                        .Build().GetConnectionString("connection");
        }

        public string Registrar(Usuario usuario)
        {
            String mensaje = "";

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegisterUsuario", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prmstrNombres", usuario.nombres);
                    cmd.Parameters.AddWithValue("@prmstrApellidos", usuario.apellidos);
                    cmd.Parameters.AddWithValue("@prmdteFechaNacimiento", usuario.fechaNacimiento);
                    cmd.Parameters.AddWithValue("@prmstrDni", usuario.dni);
                    cmd.Parameters.AddWithValue("@prmintIdRegion", usuario.idRegion);
                    cmd.Parameters.AddWithValue("@prmstrLogin", usuario.login);
                    cmd.Parameters.AddWithValue("@prmstrPassword", usuario.password);

                    connection.Open();
                    int filas = cmd.ExecuteNonQuery();
                    mensaje = $"{filas} usuario(s) registrado(s).";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                } 
                finally { connection.Close(); }
            }

            return mensaje;
        }

        public bool SeguridadUsuario(Usuario usuario)
        {
            bool existeUsuario = false;

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_SeguridadUsuario", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmstrLogin", usuario.login);
                cmd.Parameters.AddWithValue("@prmstrPassword", usuario.password);

                connection.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    existeUsuario = true;
                }
            }

            return existeUsuario;
        }
    }
}
