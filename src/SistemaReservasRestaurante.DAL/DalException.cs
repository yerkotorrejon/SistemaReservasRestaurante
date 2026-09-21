namespace SistemaReservasRestaurante.DAL
{
    // Envuelve errores SQL (RAISERROR de los SPs, timeouts, conexión) en un error
    // de negocio controlado, para que la UI nunca muestre una excepción cruda.
    public class DalException : Exception
    {
        public DalException(string message, Exception? inner = null) : base(message, inner) { }
    }
}
