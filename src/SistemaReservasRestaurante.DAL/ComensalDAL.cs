using Microsoft.Data.SqlClient;
using System.Data;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.DAL
{
    public class ComensalDAL
    {
        public int Insertar(Comensal c)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Comensal_Insertar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                cmd.Parameters.AddWithValue("@Email", (object?)c.Email ?? DBNull.Value);
                var outId = new SqlParameter("@ComensalId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outId.Value;
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
        }

        public List<Comensal> Listar(string? filtro = null)
        {
            var lista = new List<Comensal>();
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Comensal_Listar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Filtro", (object?)filtro ?? DBNull.Value);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Comensal
                    {
                        ComensalId = (int)reader["ComensalId"],
                        Nombre = (string)reader["Nombre"],
                        Telefono = (string)reader["Telefono"],
                        Email = reader["Email"] as string
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
