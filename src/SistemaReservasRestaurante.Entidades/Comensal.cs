namespace SistemaReservasRestaurante.Entidades
{
    public class Comensal
    {
        public int ComensalId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
