using SistemaReservasRestaurante.DAL;
using SistemaReservasRestaurante.Entidades;
using System.Text.RegularExpressions;

namespace SistemaReservasRestaurante.BLL
{
    public class ComensalBLL
    {
        private readonly ComensalDAL _dal = new();
        private readonly AuditoriaDAL _auditoria = new();

        public int Registrar(Comensal c)
        {
            SesionActual.RequerirSesion();
            Validar(c);

            int id;
            try
            {
                id = _dal.Insertar(c);
            }
            catch (DalException ex)
            {
                throw new BllException(ex.Message);
            }

            _auditoria.Registrar(SesionActual.Usuario!.UsuarioId, "INSERCION", "Comensal", id, c.Nombre);
            return id;
        }

        public List<Comensal> Listar(string? filtro = null) => _dal.Listar(filtro);

        private static void Validar(Comensal c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                throw new BllException("El nombre del comensal es obligatorio.");
            if (string.IsNullOrWhiteSpace(c.Telefono))
                throw new BllException("El teléfono del comensal es obligatorio.");
            if (!string.IsNullOrWhiteSpace(c.Email) && !Regex.IsMatch(c.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new BllException("El correo electrónico no tiene un formato válido.");
        }
    }
}
