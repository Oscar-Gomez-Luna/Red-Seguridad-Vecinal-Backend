using System;

namespace Backend_RSV.Models.Request
{

    public class CreateAmenidadRequest
    {
        public int TipoAmenidadID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public int? Capacidad { get; set; }
    }

    public class CreateReservaRequest
    {
        public int UsuarioID { get; set; }
        public int AmenidadID { get; set; }
        public DateOnly FechaReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string? Motivo { get; set; }
    }

    public class UpdateEstadoReservaRequest
    {
        public string Estado { get; set; } = string.Empty;
    }
}