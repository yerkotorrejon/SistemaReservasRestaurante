namespace SistemaReservasRestaurante.Entidades
{
    public class Mesa
    {
        public int MesaId { get; set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public int SectorId { get; set; }
        public string? Sector { get; set; }
        public string Estado { get; set; } = "ACTIVA";

        public string Etiqueta => $"Mesa {Numero} ({Capacidad}p) — {Sector}";
    }
}
