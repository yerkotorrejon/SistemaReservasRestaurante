using Microsoft.Data.SqlClient;
using System.Data;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.DAL
{
    public class ReservaDAL
    {
        // El procedimiento rechaza el alta si la mesa ya tiene una reserva
        // CONFIRMADA cuyo horario se traslapa (RAISERROR → SqlException).
        public int Insertar(Reserva r)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Reserva_Insertar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@ComensalId", r.ComensalId);
                cmd.Parameters.AddWithValue("@MesaId", r.MesaId);
                cmd.Parameters.AddWithValue("@FechaHoraInicio", r.FechaHoraInicio);
                cmd.Parameters.AddWithValue("@FechaHoraFin", r.FechaHoraFin);
                cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);
                cmd.Parameters.AddWithValue("@AutorUsuarioId", r.AutorUsuarioId);
                var outId = new SqlParameter("@ReservaId", SqlDbType.Int) { Direction = ParameterDirection.Output };
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

        public List<Reserva> Listar(DateTime? fecha = null, int? mesaId = null, int? comensalId = null)
        {
            var lista = new List<Reserva>();
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Reserva_Listar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Fecha", (object?)fecha?.Date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MesaId", (object?)mesaId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ComensalId", (object?)comensalId ?? DBNull.Value);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Reserva
                    {
                        ReservaId = (int)reader["ReservaId"],
                        ComensalId = (int)reader["ComensalId"],
                        Comensal = (string)reader["Comensal"],
                        Telefono = (string)reader["Telefono"],
                        MesaId = (int)reader["MesaId"],
                        NumeroMesa = (int)reader["NumeroMesa"],
                        FechaHoraInicio = (DateTime)reader["FechaHoraInicio"],
                        FechaHoraFin = (DateTime)reader["FechaHoraFin"],
                        CantidadPersonas = (int)reader["CantidadPersonas"],
                        Estado = Enum.Parse<EstadoReserva>((string)reader["Estado"]),
                        AutorUsuarioId = (int)reader["AutorUsuarioId"],
                        FechaCreacion = (DateTime)reader["FechaCreacion"]
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
            return lista;
        }

        public Reserva? ObtenerPorId(int reservaId)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Reserva_ObtenerPorId", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@ReservaId", reservaId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return new Reserva
                {
                    ReservaId = (int)reader["ReservaId"],
                    ComensalId = (int)reader["ComensalId"],
                    MesaId = (int)reader["MesaId"],
                    FechaHoraInicio = (DateTime)reader["FechaHoraInicio"],
                    FechaHoraFin = (DateTime)reader["FechaHoraFin"],
                    CantidadPersonas = (int)reader["CantidadPersonas"],
                    Estado = Enum.Parse<EstadoReserva>((string)reader["Estado"]),
                    AutorUsuarioId = (int)reader["AutorUsuarioId"],
                    FechaCreacion = (DateTime)reader["FechaCreacion"],
                    FechaCancelacion = reader["FechaCancelacion"] as DateTime?
                };
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
        }

        // Revalida disponibilidad excluyendo la propia reserva; rechaza si está CANCELADA.
        public void Actualizar(Reserva r)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Reserva_Actualizar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@ReservaId", r.ReservaId);
                cmd.Parameters.AddWithValue("@MesaId", r.MesaId);
                cmd.Parameters.AddWithValue("@FechaHoraInicio", r.FechaHoraInicio);
                cmd.Parameters.AddWithValue("@FechaHoraFin", r.FechaHoraFin);
                cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DalException(ex.Message, ex);
            }
        }

        public void Cancelar(int reservaId)
        {
            try
            {
                using var conn = ConexionDb.ObtenerConexion();
                using var cmd = new SqlCommand("sp_Reserva_Cancelar", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@ReservaId", reservaId);

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
