namespace SistemaReservasRestaurante.Entidades
{
    public enum EstadoReserva
    {
        CONFIRMADA,
        CANCELADA
    }

    public class Reserva
    {
        public int ReservaId { get; set; }
        public int ComensalId { get; set; }
        public int MesaId { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public int CantidadPersonas { get; set; }
        public EstadoReserva Estado { get; set; } = EstadoReserva.CONFIRMADA;
        public int AutorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCancelacion { get; set; }

        // Datos de contexto para listados (join con Comensal/Mesa)
        public string? Comensal { get; set; }
        public string? Telefono { get; set; }
        public int NumeroMesa { get; set; }
    }
}
