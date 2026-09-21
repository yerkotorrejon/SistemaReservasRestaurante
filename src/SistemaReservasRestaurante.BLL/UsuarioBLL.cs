using SistemaReservasRestaurante.DAL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.BLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _dal = new();

        public List<Usuario> Listar() => _dal.Listar();
    }
}
