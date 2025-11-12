using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_RSV.Controllers;

namespace Backend_RSV.Data.Servicios
{
    public class ServiciosData
    {
        private readonly AppDbContext _context;

        public ServiciosData(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. TIPOS DE SERVICIO - CORREGIDOS
        // ==========================================

        public async Task<List<TipoServicioDTO>> GetTiposServicioAsync()
        {
            return await _context.TiposServicio
                .Where(t => t.Activo)
                .Select(t => new TipoServicioDTO
                {
                    TipoServicioID = t.TipoServicioID,
                    Nombre = t.Nombre,
                    Activo = t.Activo
                })
                .ToListAsync();
        }

        public async Task<TipoServicioDTO?> GetTipoServicioByIdAsync(int id)
        {
            return await _context.TiposServicio
                .Where(t => t.TipoServicioID == id && t.Activo)
                .Select(t => new TipoServicioDTO
                {
                    TipoServicioID = t.TipoServicioID,
                    Nombre = t.Nombre,
                    Activo = t.Activo
                })
                .FirstOrDefaultAsync();
        }

        // ==========================================
        // 2. CATÁLOGO DE SERVICIOS - CORREGIDOS
        // ==========================================

        public async Task<List<ServicioCatalogoDTO>> GetServiciosCatalogoAsync()
        {
            return await _context.ServiciosCatalogo
                .Where(s => s.Activo)
                .Select(s => new ServicioCatalogoDTO
                {
                    ServicioID = s.ServicioID,
                    TipoServicioID = s.TipoServicioID,
                    NombreEncargado = s.NombreEncargado,
                    Telefono = s.Telefono,
                    Email = s.Email,
                    NumeroServiciosCompletados = s.NumeroServiciosCompletados,
                    Disponible = s.Disponible,
                    NotasInternas = s.NotasInternas,
                    FechaRegistro = s.FechaRegistro,
                    Activo = s.Activo,
                    TipoServicioNombre = s.TipoServicio.Nombre
                })
                .ToListAsync();
        }

        public async Task<ServicioCatalogoDTO?> GetServicioCatalogoByIdAsync(int id)
        {
            return await _context.ServiciosCatalogo
                .Where(s => s.ServicioID == id && s.Activo)
                .Select(s => new ServicioCatalogoDTO
                {
                    ServicioID = s.ServicioID,
                    TipoServicioID = s.TipoServicioID,
                    NombreEncargado = s.NombreEncargado,
                    Telefono = s.Telefono,
                    Email = s.Email,
                    NumeroServiciosCompletados = s.NumeroServiciosCompletados,
                    Disponible = s.Disponible,
                    NotasInternas = s.NotasInternas,
                    FechaRegistro = s.FechaRegistro,
                    Activo = s.Activo,
                    TipoServicioNombre = s.TipoServicio.Nombre
                })
                .FirstOrDefaultAsync();
        }

        // ==========================================
        // 3. PERSONAL MANTENIMIENTO - CORREGIDOS
        // ==========================================

        public async Task<List<PersonalMantenimientoDTO>> GetPersonalMantenimientoAsync()
        {
            return await _context.PersonalMantenimiento
                .Where(p => p.Activo)
                .Select(p => new PersonalMantenimientoDTO
                {
                    PersonalMantenimientoID = p.PersonalMantenimientoID,
                    PersonaID = p.PersonaID,
                    Puesto = p.Puesto,
                    FechaContratacion = p.FechaContratacion,
                    Sueldo = p.Sueldo,
                    TipoContrato = p.TipoContrato,
                    Turno = p.Turno,
                    DiasLaborales = p.DiasLaborales,
                    Activo = p.Activo,
                    Notas = p.Notas,
                    NombrePersona = p.Persona.Nombre + " " + p.Persona.ApellidoPaterno,
                    TelefonoPersona = p.Persona.Telefono
                })
                .ToListAsync();
        }

        public async Task<PersonalMantenimientoDTO?> GetPersonalMantenimientoByIdAsync(int id)
        {
            return await _context.PersonalMantenimiento
                .Where(p => p.PersonalMantenimientoID == id && p.Activo)
                .Select(p => new PersonalMantenimientoDTO
                {
                    PersonalMantenimientoID = p.PersonalMantenimientoID,
                    PersonaID = p.PersonaID,
                    Puesto = p.Puesto,
                    FechaContratacion = p.FechaContratacion,
                    Sueldo = p.Sueldo,
                    TipoContrato = p.TipoContrato,
                    Turno = p.Turno,
                    DiasLaborales = p.DiasLaborales,
                    Activo = p.Activo,
                    Notas = p.Notas,
                    NombrePersona = p.Persona.Nombre + " " + p.Persona.ApellidoPaterno,
                    TelefonoPersona = p.Persona.Telefono,
                    EmailPersona = p.Persona.Email
                })
                .FirstOrDefaultAsync();
        }

        // ==========================================
        // 4. SOLICITUDES DE SERVICIO - CORREGIDOS
        // ==========================================

        public async Task<List<SolicitudServicioDTO>> GetSolicitudesAsync()
        {
            return await _context.SolicitudesServicio
                .Select(s => new SolicitudServicioDTO
                {
                    SolicitudID = s.SolicitudID,
                    UsuarioID = s.UsuarioID,
                    TipoServicioID = s.TipoServicioID,
                    PersonaAsignado = s.PersonaAsignado,
                    Descripcion = s.Descripcion,
                    Urgencia = s.Urgencia,
                    FechaPreferida = s.FechaPreferida,
                    HoraPreferida = s.HoraPreferida,
                    Estado = s.Estado,
                    FechaCreacion = s.FechaCreacion,
                    FechaAsignacion = s.FechaAsignacion,
                    FechaCompletado = s.FechaCompletado,
                    NotasAdmin = s.NotasAdmin,
                    NombreUsuario = s.Usuario.Persona.Nombre + " " + s.Usuario.Persona.ApellidoPaterno,
                    TipoServicioNombre = s.TipoServicio.Nombre,
                    NombreAsignado = s.ServicioAsignado != null ? s.ServicioAsignado.NombreEncargado : null
                })
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();
        }

        public async Task<SolicitudServicioDTO?> GetSolicitudByIdAsync(int id)
        {
            return await _context.SolicitudesServicio
                .Where(s => s.SolicitudID == id)
                .Select(s => new SolicitudServicioDTO
                {
                    SolicitudID = s.SolicitudID,
                    UsuarioID = s.UsuarioID,
                    TipoServicioID = s.TipoServicioID,
                    PersonaAsignado = s.PersonaAsignado,
                    Descripcion = s.Descripcion,
                    Urgencia = s.Urgencia,
                    FechaPreferida = s.FechaPreferida,
                    HoraPreferida = s.HoraPreferida,
                    Estado = s.Estado,
                    FechaCreacion = s.FechaCreacion,
                    FechaAsignacion = s.FechaAsignacion,
                    FechaCompletado = s.FechaCompletado,
                    NotasAdmin = s.NotasAdmin,
                    NombreUsuario = s.Usuario.Persona.Nombre + " " + s.Usuario.Persona.ApellidoPaterno,
                    EmailUsuario = s.Usuario.Persona.Email,
                    TelefonoUsuario = s.Usuario.Persona.Telefono,
                    TipoServicioNombre = s.TipoServicio.Nombre,
                    NombreAsignado = s.ServicioAsignado != null ? s.ServicioAsignado.NombreEncargado : null,
                    TelefonoAsignado = s.ServicioAsignado != null ? s.ServicioAsignado.Telefono : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<SolicitudServicioDTO>> GetSolicitudesByUsuarioAsync(int usuarioId)
        {
            return await _context.SolicitudesServicio
                .Where(s => s.UsuarioID == usuarioId)
                .Select(s => new SolicitudServicioDTO
                {
                    SolicitudID = s.SolicitudID,
                    UsuarioID = s.UsuarioID,
                    TipoServicioID = s.TipoServicioID,
                    PersonaAsignado = s.PersonaAsignado,
                    Descripcion = s.Descripcion,
                    Urgencia = s.Urgencia,
                    FechaPreferida = s.FechaPreferida,
                    HoraPreferida = s.HoraPreferida,
                    Estado = s.Estado,
                    FechaCreacion = s.FechaCreacion,
                    FechaAsignacion = s.FechaAsignacion,
                    FechaCompletado = s.FechaCompletado,
                    NotasAdmin = s.NotasAdmin,
                    TipoServicioNombre = s.TipoServicio.Nombre,
                    NombreAsignado = s.ServicioAsignado != null ? s.ServicioAsignado.NombreEncargado : null
                })
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();
        }

        // ==========================================
        // 5. CARGOS SERVICIO - CORREGIDOS
        // ==========================================

        public async Task<List<CargoServicioDTO>> GetCargosServiciosByUsuarioAsync(int usuarioId)
        {
            return await _context.CargosServicios
                .Where(c => c.UsuarioID == usuarioId)
                .Select(c => new CargoServicioDTO
                {
                    CargoServicioID = c.CargoServicioID,
                    UsuarioID = c.UsuarioID,
                    SolicitudID = c.SolicitudID,
                    Concepto = c.Concepto,
                    Monto = c.Monto,
                    Estado = c.Estado,
                    MontoPagado = c.MontoPagado,
                    SaldoPendiente = c.SaldoPendiente,
                    FechaCreacion = c.FechaCreacion,
                    DescripcionSolicitud = c.Solicitud.Descripcion
                })
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<CargoServicioDTO>> GetCargosServiciosBySolicitudAsync(int solicitudId)
        {
            return await _context.CargosServicios
                .Where(c => c.SolicitudID == solicitudId)
                .Select(c => new CargoServicioDTO
                {
                    CargoServicioID = c.CargoServicioID,
                    UsuarioID = c.UsuarioID,
                    SolicitudID = c.SolicitudID,
                    Concepto = c.Concepto,
                    Monto = c.Monto,
                    Estado = c.Estado,
                    MontoPagado = c.MontoPagado,
                    SaldoPendiente = c.SaldoPendiente,
                    FechaCreacion = c.FechaCreacion,
                    NombreUsuario = c.Usuario.Persona.Nombre + " " + c.Usuario.Persona.ApellidoPaterno
                })
                .ToListAsync();
        }

        // ==========================================
        // 6. CARGOS MANTENIMIENTO - CORREGIDOS
        // ==========================================

        public async Task<List<CargoMantenimientoDTO>> GetCargosMantenimientoAsync()
        {
            return await _context.CargosMantenimiento
                .Select(c => new CargoMantenimientoDTO
                {
                    CargoMantenimientoID = c.CargoMantenimientoID,
                    UsuarioID = c.UsuarioID,
                    PersonalMantenimientoID = c.PersonalMantenimientoID,
                    Concepto = c.Concepto,
                    Monto = c.Monto,
                    FechaVencimiento = c.FechaVencimiento,
                    Estado = c.Estado,
                    MontoPagado = c.MontoPagado,
                    SaldoPendiente = c.SaldoPendiente,
                    FechaCreacion = c.FechaCreacion,
                    NombreUsuario = c.Usuario != null ? c.Usuario.Persona.Nombre + " " + c.Usuario.Persona.ApellidoPaterno : "General",
                    NombrePersonal = c.PersonalMantenimiento != null ? c.PersonalMantenimiento.Persona.Nombre + " " + c.PersonalMantenimiento.Persona.ApellidoPaterno : null
                })
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<CargoMantenimientoDTO>> GetCargosMantenimientoByUsuarioAsync(int usuarioId)
        {
            return await _context.CargosMantenimiento
                .Where(c => c.UsuarioID == usuarioId)
                .Select(c => new CargoMantenimientoDTO
                {
                    CargoMantenimientoID = c.CargoMantenimientoID,
                    UsuarioID = c.UsuarioID,
                    PersonalMantenimientoID = c.PersonalMantenimientoID,
                    Concepto = c.Concepto,
                    Monto = c.Monto,
                    FechaVencimiento = c.FechaVencimiento,
                    Estado = c.Estado,
                    MontoPagado = c.MontoPagado,
                    SaldoPendiente = c.SaldoPendiente,
                    FechaCreacion = c.FechaCreacion,
                    NombrePersonal = c.PersonalMantenimiento != null ? c.PersonalMantenimiento.Persona.Nombre + " " + c.PersonalMantenimiento.Persona.ApellidoPaterno : null
                })
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        // ==========================================
        // MÉTODOS CREATE/UPDATE (se mantienen igual)
        // ==========================================

        public async Task<ServiciosCatalogo> CreateServicioCatalogoAsync(ServicioCatalogoRequest request)
        {
            var servicio = new ServiciosCatalogo
            {
                TipoServicioID = request.TipoServicioID,
                NombreEncargado = request.NombreEncargado,
                Telefono = request.Telefono,
                Email = request.Email,
                NotasInternas = request.NotasInternas,
                Disponible = request.Disponible,
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            _context.ServiciosCatalogo.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<ServiciosCatalogo?> UpdateServicioCatalogoAsync(int id, ServicioCatalogoRequest request)
        {
            var servicio = await _context.ServiciosCatalogo
                .FirstOrDefaultAsync(s => s.ServicioID == id && s.Activo);

            if (servicio == null) return null;

            servicio.TipoServicioID = request.TipoServicioID;
            servicio.NombreEncargado = request.NombreEncargado;
            servicio.Telefono = request.Telefono;
            servicio.Email = request.Email;
            servicio.NotasInternas = request.NotasInternas;
            servicio.Disponible = request.Disponible;

            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<ServiciosCatalogo?> UpdateDisponibilidadServicioAsync(int id, bool disponible)
        {
            var servicio = await _context.ServiciosCatalogo
                .FirstOrDefaultAsync(s => s.ServicioID == id && s.Activo);

            if (servicio == null) return null;

            servicio.Disponible = disponible;
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task<PersonalMantenimiento> CreatePersonalMantenimientoAsync(PersonalMantenimientoRequest request)
        {
            var personal = new PersonalMantenimiento
            {
                PersonaID = request.PersonaID,
                Puesto = request.Puesto,
                FechaContratacion = request.FechaContratacion,
                Sueldo = request.Sueldo,
                TipoContrato = request.TipoContrato,
                Turno = request.Turno,
                DiasLaborales = request.DiasLaborales,
                Notas = request.Notas,
                Activo = true
            };

            _context.PersonalMantenimiento.Add(personal);
            await _context.SaveChangesAsync();
            return personal;
        }

        public async Task<SolicitudesServicio> CreateSolicitudAsync(SolicitudServicioRequest request)
        {
            var solicitud = new SolicitudesServicio
            {
                UsuarioID = request.UsuarioID,
                TipoServicioID = request.TipoServicioID,
                Descripcion = request.Descripcion,
                Urgencia = request.Urgencia,
                FechaPreferida = request.FechaPreferida,
                HoraPreferida = request.HoraPreferida,
                Estado = "Pendiente",
                FechaCreacion = DateTime.Now
            };

            _context.SolicitudesServicio.Add(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<SolicitudesServicio?> AsignarSolicitudAsync(int id, int personaAsignado)
        {
            var solicitud = await _context.SolicitudesServicio
                .FirstOrDefaultAsync(s => s.SolicitudID == id);

            if (solicitud == null) return null;

            solicitud.PersonaAsignado = personaAsignado;
            solicitud.Estado = "Asignado";
            solicitud.FechaAsignacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<SolicitudesServicio?> UpdateEstadoSolicitudAsync(int id, string estado)
        {
            var solicitud = await _context.SolicitudesServicio
                .FirstOrDefaultAsync(s => s.SolicitudID == id);

            if (solicitud == null) return null;

            solicitud.Estado = estado;
            if (estado == "Completado") solicitud.FechaCompletado = DateTime.Now;

            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<SolicitudesServicio?> CompletarSolicitudAsync(int id, string? notasAdmin)
        {
            var solicitud = await _context.SolicitudesServicio
                .FirstOrDefaultAsync(s => s.SolicitudID == id);

            if (solicitud == null) return null;

            solicitud.Estado = "Completado";
            solicitud.NotasAdmin = notasAdmin;
            solicitud.FechaCompletado = DateTime.Now;

            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<CargoMantenimiento> CreateCargoMantenimientoAsync(CargoMantenimientoRequest request)
        {
            var cargo = new CargoMantenimiento
            {
                UsuarioID = request.UsuarioID,
                PersonalMantenimientoID = request.PersonalMantenimientoID,
                Concepto = request.Concepto,
                Monto = request.Monto,
                FechaVencimiento = request.FechaVencimiento,
                Estado = "Pendiente",
                FechaCreacion = DateTime.Now
            };

            _context.CargosMantenimiento.Add(cargo);
            await _context.SaveChangesAsync();
            return cargo;
        }

        public async Task<CargoMantenimiento?> UpdateCargoMantenimientoAsync(int id, CargoMantenimientoRequest request)
        {
            var cargo = await _context.CargosMantenimiento
                .FirstOrDefaultAsync(c => c.CargoMantenimientoID == id);

            if (cargo == null) return null;

            cargo.UsuarioID = request.UsuarioID;
            cargo.PersonalMantenimientoID = request.PersonalMantenimientoID;
            cargo.Concepto = request.Concepto;
            cargo.Monto = request.Monto;
            cargo.FechaVencimiento = request.FechaVencimiento;

            await _context.SaveChangesAsync();
            return cargo;
        }
    }

    // ==========================================
    // DTOs PARA EVITAR REFERENCIAS CIRCULARES
    // ==========================================

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