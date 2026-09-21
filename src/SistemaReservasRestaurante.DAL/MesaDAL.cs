using Microsoft.Data.SqlClient;
using System.Data;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.DAL
{
    public class MesaDAL
    {
        public int Insertar(Mesa m)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Mesa_Insertar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Numero", m.Numero);
                cmd.Parameters.AddWithValue("@Capacidad", m.Capacidad);
                cmd.Parameters.AddWithValue("@SectorId", m.SectorId);
                var outId = new SqlParameter("@MesaId", SqlDbType.Int) { Direction = ParameterDirection.Output };
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

        public List<Mesa> Listar()
        {
            var lista = new List<Mesa>();
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Mesa_Listar", conn) { CommandType = CommandType.StoredProcedure };

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Mesa
                    {
                        MesaId = (int)reader["MesaId"],
                        Numero = (int)reader["Numero"],
                        Capacidad = (int)reader["Capacidad"],
                        SectorId = (int)reader["SectorId"],
                        Sector = (string)reader["Sector"],
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

        public void Actualizar(Mesa m)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Mesa_Actualizar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@MesaId", m.MesaId);
                cmd.Parameters.AddWithValue("@Capacidad", m.Capacidad);
                cmd.Parameters.AddWithValue("@SectorId", m.SectorId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
        }

        public void Anular(int mesaId)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Mesa_Anular", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@MesaId", mesaId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
        }
    }
}
