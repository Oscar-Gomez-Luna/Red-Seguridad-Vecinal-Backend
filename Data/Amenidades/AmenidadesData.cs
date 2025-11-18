using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_RSV.Data.Alertas;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Data.Amenidades
{
    public class AmenidadesData
    {
        private readonly AppDbContext _context;

        public AmenidadesData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AmenidadDTO>> GetAmenidadesAsync()
        {
            return await _context.Amenidades
                .Include(a => a.TipoAmenidad)
                .Where(a => a.Activo)
                .Select(a => new AmenidadDTO
                {
                    AmenidadID = a.AmenidadID,
                    TipoAmenidadID = a.TipoAmenidadID,
                    Nombre = a.Nombre,
                    Ubicacion = a.Ubicacion,
                    Capacidad = a.Capacidad,
                    Activo = a.Activo,
                    TipoAmenidadNombre = a.TipoAmenidad.Nombre,
                    HorarioInicio = a.TipoAmenidad.HorarioInicio,
                    HorarioFin = a.TipoAmenidad.HorarioFin
                })
                .ToListAsync();
        }

        public async Task<AmenidadDTO?> GetAmenidadByIdAsync(int id)
        {
            return await _context.Amenidades
                .Include(a => a.TipoAmenidad)
                .Where(a => a.AmenidadID == id && a.Activo)
                .Select(a => new AmenidadDTO
                {
                    AmenidadID = a.AmenidadID,
                    TipoAmenidadID = a.TipoAmenidadID,
                    Nombre = a.Nombre,
                    Ubicacion = a.Ubicacion,
                    Capacidad = a.Capacidad,
                    Activo = a.Activo,
                    TipoAmenidadNombre = a.TipoAmenidad.Nombre,
                    HorarioInicio = a.TipoAmenidad.HorarioInicio,
                    HorarioFin = a.TipoAmenidad.HorarioFin
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<TipoAmenidadDTO>> GetTiposAmenidadAsync()
        {
            return await _context.TiposAmenidad
                .Where(t => t.Activo)
                .Select(t => new TipoAmenidadDTO
                {
                    TipoAmenidadID = t.TipoAmenidadID,
                    Nombre = t.Nombre,
                    HorarioInicio = t.HorarioInicio,
                    HorarioFin = t.HorarioFin,
                    Activo = t.Activo
                })
                .ToListAsync();
        }

        public async Task<Amenidad> CreateAmenidadAsync(CreateAmenidadRequest request)
        {
            var amenidad = new Amenidad
            {
                TipoAmenidadID = request.TipoAmenidadID,
                Nombre = request.Nombre,
                Ubicacion = request.Ubicacion,
                Capacidad = request.Capacidad,
                Activo = true
            };

            _context.Amenidades.Add(amenidad);
            await _context.SaveChangesAsync();
            return amenidad;
        }

        public async Task<Reserva> CreateReservaAsync(CreateReservaRequest request)
        {
            var reserva = new Reserva
            {
                UsuarioID = request.UsuarioID,
                AmenidadID = request.AmenidadID,
                FechaReserva = request.FechaReserva,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                Motivo = request.Motivo,
                FechaCreacion = DateTime.Now,
                Estado = "Pendiente"
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }


        public async Task<List<ReservaDTO>> GetReservasByUsuarioAsync(int usuarioId)
        {
            return await _context.Reservas
                .Include(r => r.Amenidad)
                    .ThenInclude(a => a.TipoAmenidad)
                .Include(r => r.Usuario)
                    .ThenInclude(u => u.Persona)
                .Where(r => r.UsuarioID == usuarioId)
                .OrderByDescending(r => r.FechaCreacion)
                .Select(r => new ReservaDTO
                {
                    ReservaID = r.ReservaID,
                    UsuarioID = r.UsuarioID,
                    AmenidadID = r.AmenidadID,
                    FechaReserva = r.FechaReserva,
                    HoraInicio = r.HoraInicio,
                    HoraFin = r.HoraFin,
                    Motivo = r.Motivo,
                    FechaCreacion = r.FechaCreacion,
                    AmenidadNombre = r.Amenidad.Nombre,
                    TipoAmenidad = r.Amenidad.TipoAmenidad.Nombre,
                    NombreUsuario = r.Usuario.Persona.Nombre + " " + r.Usuario.Persona.ApellidoPaterno,
                    Estado = r.Estado
                })
                .ToListAsync();
        }


        public async Task<List<ReservaDTO>> GetAllReservasAsync()
        {
            return await _context.Reservas
                .Include(r => r.Amenidad)
                    .ThenInclude(a => a.TipoAmenidad)
                .Include(r => r.Usuario)
                    .ThenInclude(u => u.Persona)
                .OrderByDescending(r => r.FechaCreacion)
                .Select(r => new ReservaDTO
                {
                    ReservaID = r.ReservaID,
                    UsuarioID = r.UsuarioID,
                    AmenidadID = r.AmenidadID,
                    FechaReserva = r.FechaReserva,
                    HoraInicio = r.HoraInicio,
                    HoraFin = r.HoraFin,
                    Motivo = r.Motivo,
                    FechaCreacion = r.FechaCreacion,
                    AmenidadNombre = r.Amenidad.Nombre,
                    TipoAmenidad = r.Amenidad.TipoAmenidad.Nombre,
                    NombreUsuario = r.Usuario.Persona.Nombre + " " + r.Usuario.Persona.ApellidoPaterno,
                    NumeroCasa = r.Usuario.NumeroCasa,
                    Estado = r.Estado
                })
                .ToListAsync();
        }


        public async Task<ReservaDTO?> GetReservaByIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Amenidad)
                    .ThenInclude(a => a.TipoAmenidad)
                .Include(r => r.Usuario)
                    .ThenInclude(u => u.Persona)
                .Where(r => r.ReservaID == id)
                .Select(r => new ReservaDTO
                {
                    ReservaID = r.ReservaID,
                    UsuarioID = r.UsuarioID,
                    AmenidadID = r.AmenidadID,
                    FechaReserva = r.FechaReserva,
                    HoraInicio = r.HoraInicio,
                    HoraFin = r.HoraFin,
                    Motivo = r.Motivo,
                    FechaCreacion = r.FechaCreacion,
                    AmenidadNombre = r.Amenidad.Nombre,
                    TipoAmenidad = r.Amenidad.TipoAmenidad.Nombre,
                    NombreUsuario = r.Usuario.Persona.Nombre + " " + r.Usuario.Persona.ApellidoPaterno,
                    EmailUsuario = r.Usuario.Persona.Email,
                    TelefonoUsuario = r.Usuario.Persona.Telefono,
                    NumeroCasa = r.Usuario.NumeroCasa,
                    Estado = r.Estado
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> CancelarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId);

            if (reserva == null)
                return false;

            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateEstadoReservaAsync(int reservaId, string estado)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaID == reservaId);

            if (reserva == null)
                return false;

            reserva.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}