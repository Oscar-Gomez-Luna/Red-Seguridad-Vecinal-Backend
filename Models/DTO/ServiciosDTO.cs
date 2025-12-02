using System;

namespace Backend_RSV.Models.DTO
{
    public class TipoServicioDTO
    {
        public int TipoServicioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class ServicioCatalogoDTO
    {
        public int ServicioID { get; set; }
        public int TipoServicioID { get; set; }
        public string NombreEncargado { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int NumeroServiciosCompletados { get; set; }
        public bool Disponible { get; set; }
        public string? NotasInternas { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public string TipoServicioNombre { get; set; } = string.Empty;
    }

 // En Backend_RSV.Models.DTO
public class PersonalMantenimientoDTO
{
    public int PersonalMantenimientoID { get; set; }
    public int PersonaID { get; set; }
    public string Puesto { get; set; } = string.Empty;
    public DateOnly FechaContratacion { get; set; }
    public decimal Sueldo { get; set; }
    public string? TipoContrato { get; set; }
    public string? Turno { get; set; }
    public string? DiasLaborales { get; set; }
    public bool Activo { get; set; }
    public string? Notas { get; set; }
    public string NombrePersona { get; set; } = string.Empty;
    public string TelefonoPersona { get; set; } = string.Empty;
    public string? EmailPersona { get; set; }
    
    // Datos completos de la persona
    public string? Nombre { get; set; }
    public string? ApellidoPaterno { get; set; }
    public string? ApellidoMaterno { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
}

    public class SolicitudServicioDTO
    {
        public int SolicitudID { get; set; }
        public int UsuarioID { get; set; }
        public int TipoServicioID { get; set; }
        public int? PersonaAsignado { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Urgencia { get; set; } = string.Empty;
        public DateOnly? FechaPreferida { get; set; }
        public TimeSpan? HoraPreferida { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public string? NotasAdmin { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string? EmailUsuario { get; set; }
        public string? TelefonoUsuario { get; set; }
        public string TipoServicioNombre { get; set; } = string.Empty;
        public string? NombreAsignado { get; set; }
        public string? TelefonoAsignado { get; set; }
    }

    public class CargoServicioDTO
    {
        public int CargoServicioID { get; set; }
        public int UsuarioID { get; set; }
        public int SolicitudID { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? DescripcionSolicitud { get; set; }
        public string? NombreUsuario { get; set; }
    }

    public class CargoMantenimientoDTO
    {
        public int CargoMantenimientoID { get; set; }
        public int? UsuarioID { get; set; }
        public int? PersonalMantenimientoID { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombrePersonal { get; set; }
    }
}