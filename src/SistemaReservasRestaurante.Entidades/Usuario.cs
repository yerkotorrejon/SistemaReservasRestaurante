namespace SistemaReservasRestaurante.Entidades
{
    public enum RolUsuario
    {
        ENCARGADO,
        ADMINISTRACION
    }

    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int RolId { get; set; }
        public RolUsuario Rol { get; set; }
        public string Estado { get; set; } = "ACTIVO";
    }
}
