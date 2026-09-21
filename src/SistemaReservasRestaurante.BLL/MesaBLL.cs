using SistemaReservasRestaurante.DAL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.BLL
{
    // Mantener el catálogo de mesas es exclusivo de Administración.
    public class MesaBLL
    {
        private readonly MesaDAL _dal = new();
        private readonly AuditoriaDAL _auditoria = new();

        public int Registrar(Mesa m)
        {
            SesionActual.RequerirAdministracion();
            Validar(m);

            int id;
            try
            {
                id = _dal.Insertar(m);
            }
            catch (DalException ex)
            {
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "INSERCION", "Mesa", id, $"Mesa N° {m.Numero}");
            return id;
        }

        public List<Mesa> Listar() => _dal.Listar();

        public void Actualizar(Mesa m)
        {
            SesionActual.RequerirAdministracion();
            Validar(m);

            try
            {
                _dal.Actualizar(m);
            }
            catch (DalException ex)
            {
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "ACTUALIZACION", "Mesa", m.MesaId, null);
        }

        public void Anular(int mesaId)
        {
            SesionActual.RequerirAdministracion();

            try
            {
                _dal.Anular(mesaId);
            }
            catch (DalException ex)
            {
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "ANULACION", "Mesa", mesaId, null);
        }

        private static void Validar(Mesa m)
        {
            if (m.Numero <= 0)
                throw new BllException("El número de mesa debe ser mayor a cero.");
            if (m.Capacidad <= 0)
                throw new BllException("La capacidad de la mesa debe ser mayor a cero.");
            if (m.SectorId <= 0)
                throw new BllException("Debe seleccionar un sector.");
        }
    }
}
