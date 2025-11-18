using System;

namespace Backend_RSV.Data.Alertas
{
    public class AmenidadDTO
    {
        public int AmenidadID { get; set; }
        public int TipoAmenidadID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public int? Capacidad { get; set; }
        public bool Activo { get; set; }
        public string TipoAmenidadNombre { get; set; } = string.Empty;
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFin { get; set; }
    }

    public class TipoAmenidadDTO
    {
        public int TipoAmenidadID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFin { get; set; }
        public bool Activo { get; set; }
    }

    public class ReservaDTO
    {
        public int ReservaID { get; set; }
        public int UsuarioID { get; set; }
        public int AmenidadID { get; set; }
        public DateOnly FechaReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Motivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string AmenidadNombre { get; set; } = string.Empty;
        public string TipoAmenidad { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string? EmailUsuario { get; set; }
        public string? TelefonoUsuario { get; set; }
        public string? NumeroCasa { get; set; }
    }
}