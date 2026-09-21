using Microsoft.Data.SqlClient;
using System.Data;

namespace SistemaReservasRestaurante.DAL
{
    public class AuditoriaDAL
    {
        public void Registrar(int usuarioId, string accion, string entidad, int? entidadId, string? detalle)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Auditoria_Insertar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cmd.Parameters.AddWithValue("@Accion", accion);
                cmd.Parameters.AddWithValue("@Entidad", entidad);
                cmd.Parameters.AddWithValue("@EntidadId", (object?)entidadId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Detalle", (object?)detalle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OrigenEquipo", Environment.MachineName);

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
