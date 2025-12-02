using System;

namespace Backend_RSV.Models.Request
{

    public class ServicioCatalogoRequest
    {
        public int TipoServicioID { get; set; }
        public string NombreEncargado { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NotasInternas { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class UpdateDisponibilidadRequest
    {
        public bool Disponible { get; set; }
    }

// En Backend_RSV.Models.Request
public class PersonalMantenimientoRequest
{
    // Datos de la persona
    public string Nombre { get; set; } = string.Empty;

    public string ApellidoPaterno { get; set; } = string.Empty;

    public string? ApellidoMaterno { get; set; }

    public string Telefono { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateOnly? FechaNacimiento { get; set; }
    
    // Datos del personal de mantenimiento
    public string Puesto { get; set; } = string.Empty;

    public DateOnly FechaContratacion { get; set; }

    public decimal Sueldo { get; set; }

    public string? TipoContrato { get; set; }

    public string? Turno { get; set; }

    public string? DiasLaborales { get; set; }

    public string? Notas { get; set; }
}

public class UpdatePersonalMantenimientoRequest
{
    // Solo campos que se puedan modificar (no incluye datos de persona)
    public string? Puesto { get; set; }
    public DateOnly? FechaContratacion { get; set; }
    public decimal? Sueldo { get; set; }
    public string? TipoContrato { get; set; }
    public string? Turno { get; set; }
    public string? DiasLaborales { get; set; }
    public string? Notas { get; set; }
    public bool? Activo { get; set; }
}

    public class SolicitudServicioRequest
    {
        public int UsuarioID { get; set; }
        public int TipoServicioID { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Urgencia { get; set; } = "Media";
        public DateOnly? FechaPreferida { get; set; }
        public TimeSpan? HoraPreferida { get; set; }
    }

    public class AsignarSolicitudRequest
    {
        public int PersonaAsignado { get; set; }
    }

    public class UpdateEstadoSolicitudRequest
    {
        public string Estado { get; set; } = string.Empty;
    }

    public class CompletarSolicitudRequest
    {
        public string? NotasAdmin { get; set; }
    }

    public class CargoMantenimientoRequest
    {
        public int? UsuarioID { get; set; }
        public int? PersonalMantenimientoID { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateOnly FechaVencimiento { get; set; }
    }
}