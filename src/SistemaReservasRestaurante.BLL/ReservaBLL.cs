using SistemaReservasRestaurante.DAL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.BLL
{
    // Principio rector: ninguna mesa admite dos reservas confirmadas cuyos
    // horarios se traslapen. Se valida aquí (BLL) antes de invocar el DAL, y
    // el procedimiento almacenado la vuelve a validar como segunda línea de
    // defensa (defensa en profundidad).
    public class ReservaBLL
    {
        private readonly ReservaDAL _dal = new();
        private readonly MesaDAL _mesaDal = new();
        private readonly AuditoriaDAL _auditoria = new();

        public int Registrar(Reserva r)
        {
            SesionActual.RequerirSesion();
            Validar(r);
            ValidarCapacidad(r);
            RevalidarDisponibilidad(r, excluirReservaId: null);
            r.AutorUsuarioId = SesionActual.Usuario!.UsuarioId;

            int id;
            try
            {
                id = _dal.Insertar(r);
            }
            catch (DalException ex)
            {
                _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "BLOQUEO", "Reserva", null,
                    $"Intento de reserva duplicada rechazado: {ex.Message}");
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "INSERCION", "Reserva", id, null);
            return id;
        }

        public List<Reserva> Listar(DateTime? fecha = null, int? mesaId = null, int? comensalId = null)
        {
            SesionActual.RequerirSesion();
            return _dal.Listar(fecha, mesaId, comensalId);
        }

        public Reserva? ObtenerPorId(int reservaId)
        {
            SesionActual.RequerirSesion();
            return _dal.ObtenerPorId(reservaId);
        }

        public void Actualizar(Reserva r)
        {
            SesionActual.RequerirSesion();
            Validar(r);

            var actual = _dal.ObtenerPorId(r.ReservaId)
                ?? throw new BllException("La reserva no existe.");

            if (actual.Estado != EstadoReserva.CONFIRMADA)
                throw new BllException($"No se puede modificar: la reserva está {actual.Estado}.");

            ValidarCapacidad(r);
            RevalidarDisponibilidad(r, excluirReservaId: r.ReservaId);

            try
            {
                _dal.Actualizar(r);
            }
            catch (DalException ex)
            {
                _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "BLOQUEO", "Reserva", r.ReservaId,
                    $"Intento de modificación con cruce rechazado: {ex.Message}");
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "ACTUALIZACION", "Reserva", r.ReservaId, null);
        }

        // Baja lógica únicamente: nunca se elimina físicamente una reserva.
        public void Cancelar(int reservaId)
        {
            SesionActual.RequerirSesion();

            try
            {
                _dal.Cancelar(reservaId);
            }
            catch (DalException ex)
            {
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "CANCELACION", "Reserva", reservaId, null);
        }

        // Defensa en la capa de lógica de negocio: revisa localmente el mismo
        // criterio de traslape que el procedimiento almacenado volverá a exigir.
        private void RevalidarDisponibilidad(Reserva r, int? excluirReservaId)
        {
            var reservasMesa = _dal.Listar(mesaId: r.MesaId)
                .Where(x => x.Estado == EstadoReserva.CONFIRMADA && x.ReservaId != excluirReservaId);

            bool haySolape = reservasMesa.Any(x =>
                r.FechaHoraInicio < x.FechaHoraFin && r.FechaHoraFin > x.FechaHoraInicio);

            if (haySolape)
                throw new BllException("La mesa ya tiene una reserva confirmada que se traslapa con ese horario.");
        }

        private void ValidarCapacidad(Reserva r)
        {
            var mesa = _mesaDal.Listar().FirstOrDefault(m => m.MesaId == r.MesaId)
                ?? throw new BllException("La mesa seleccionada no existe o está inactiva.");

            if (r.CantidadPersonas > mesa.Capacidad)
                throw new BllException($"La mesa {mesa.Numero} tiene capacidad para {mesa.Capacidad} personas.");
        }

        private static void Validar(Reserva r)
        {
            if (r.ComensalId <= 0)
                throw new BllException("Debe seleccionar un comensal.");
            if (r.MesaId <= 0)
                throw new BllException("Debe seleccionar una mesa.");
            if (r.FechaHoraFin <= r.FechaHoraInicio)
                throw new BllException("La hora de término debe ser posterior a la hora de inicio.");
            if (r.FechaHoraInicio < DateTime.Now.AddMinutes(-5))
                throw new BllException("No se pueden registrar reservas en el pasado.");
            if (r.CantidadPersonas <= 0)
                throw new BllException("La cantidad de personas debe ser mayor a cero.");
        }
    }
}
