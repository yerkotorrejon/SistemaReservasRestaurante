using Microsoft.Data.SqlClient;
using System.Data;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.DAL
{
    public class UsuarioDAL
    {
        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Usuario_Listar", conn) { CommandType = CommandType.StoredProcedure };

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        UsuarioId = (int)reader["UsuarioId"],
                        NombreUsuario = (string)reader["NombreUsuario"],
                        RolId = (int)reader["RolId"],
                        Rol = Enum.Parse<RolUsuario>((string)reader["Rol"]),
                        Estado = (string)reader["Estado"]
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
            return lista;
        }
    }
}
