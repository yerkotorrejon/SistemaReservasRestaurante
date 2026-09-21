using SistemaReservasRestaurante.DAL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.BLL
{
    public class SectorBLL
    {
        private readonly SectorDAL _dal = new();

        public List<Sector> Listar() => _dal.Listar();
    }
}
