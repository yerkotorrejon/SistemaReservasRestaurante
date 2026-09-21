using Microsoft.Data.SqlClient;
using System.Data;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.DAL
{
    public class SectorDAL
    {
        public List<Sector> Listar()
        {
            var lista = new List<Sector>();
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Sector_Listar", conn) { CommandType = CommandType.StoredProcedure };

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Sector
                    {
                        SectorId = (int)reader["SectorId"],
                        Nombre = (string)reader["Nombre"]
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
