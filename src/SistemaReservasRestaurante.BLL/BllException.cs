namespace SistemaReservasRestaurante.BLL
{
    // Error de regla de negocio (validación de campos, permisos, disponibilidad).
    // Se distingue de DalException para que la UI pueda mostrar un mensaje
    // de negocio limpio en vez de un error técnico de base de datos.
    public class BllException : Exception
    {
        public BllException(string message) : base(message) { }
    }
}
