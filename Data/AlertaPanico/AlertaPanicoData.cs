using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Data.Alertas
{
    public class AlertaPanicoData
    {
        private readonly AppDbContext _context;

        public AlertaPanicoData(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AlertaPanicoDTO>> GetAlertasAsync()
        {
            return await _context.AlertasPanico
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.Persona)
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.TipoUsuario)
                .OrderByDescending(a => a.FechaHora)
                .Select(a => new AlertaPanicoDTO
                {
                    AlertaID = a.AlertaID,
                    UsuarioID = a.UsuarioID,
                    Latitud = a.Latitud,
                    Longitud = a.Longitud,
                    FechaHora = a.FechaHora,
                    NombreUsuario = a.Usuario.Persona.Nombre + " " + a.Usuario.Persona.ApellidoPaterno,
                    EmailUsuario = a.Usuario.Persona.Email,
                    TipoUsuario = a.Usuario.TipoUsuario.Nombre
                })
                .ToListAsync();
        }
        public async Task<AlertaPanicoDetailDTO?> GetAlertaByIdAsync(int id)
        {
            return await _context.AlertasPanico
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.Persona)
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.TipoUsuario)
                .Where(a => a.AlertaID == id)
                .Select(a => new AlertaPanicoDetailDTO
                {
                    AlertaID = a.AlertaID,
                    UsuarioID = a.UsuarioID,
                    Latitud = a.Latitud,
                    Longitud = a.Longitud,
                    FechaHora = a.FechaHora,
                    NombreCompleto = a.Usuario.Persona.Nombre + " " +
                                   a.Usuario.Persona.ApellidoPaterno + " " +
                                   a.Usuario.Persona.ApellidoMaterno,
                    Email = a.Usuario.Persona.Email,
                    Telefono = a.Usuario.Persona.Telefono,
                    TipoUsuario = a.Usuario.TipoUsuario.Nombre,
                    NumeroCasa = a.Usuario.NumeroCasa ?? "",
                    Calle = a.Usuario.Calle ?? ""
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<AlertaPanicoDTO>> GetAlertasByUsuarioAsync(int usuarioId)
        {
            return await _context.AlertasPanico
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.Persona)
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.TipoUsuario)
                .Where(a => a.UsuarioID == usuarioId)
                .OrderByDescending(a => a.FechaHora)
                .Select(a => new AlertaPanicoDTO
                {
                    AlertaID = a.AlertaID,
                    UsuarioID = a.UsuarioID,
                    Latitud = a.Latitud,
                    Longitud = a.Longitud,
                    FechaHora = a.FechaHora,
                    NombreUsuario = a.Usuario.Persona.Nombre + " " + a.Usuario.Persona.ApellidoPaterno,
                    EmailUsuario = a.Usuario.Persona.Email,
                    TipoUsuario = a.Usuario.TipoUsuario.Nombre
                })
                .ToListAsync();
        }

        public async Task<AlertaPanico> CreateAlertaAsync(AlertaPanicoRequest request)
        {
            var alerta = new AlertaPanico
            {
                UsuarioID = request.UsuarioID,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                FechaHora = DateTime.Now
            };

            _context.AlertasPanico.Add(alerta);
            await _context.SaveChangesAsync();

            await _context.Entry(alerta)
                .Reference(a => a.Usuario)
                .Query()
                .Include(u => u.Persona)
                .Include(u => u.TipoUsuario)
                .LoadAsync();

            return alerta;
        }
        // Actualizar coordenadas
        public async Task UpdateAlertaCoordenadasAsync(int alertaId, decimal lat, decimal lng)
        {
            var alerta = await _context.AlertasPanico.FindAsync(alertaId);
            if (alerta != null)
            {
                alerta.Latitud = lat;
                alerta.Longitud = lng;
                await _context.SaveChangesAsync();
            }
        }

        // Actualizar estatus (solo sincronización, no requiere campo nuevo si no existe)
        public async Task UpdateAlertaEstatusAsync(int alertaId, string estatus)
        {
            var alerta = await _context.AlertasPanico.FindAsync(alertaId);
            if (alerta != null)
            {
                await _context.SaveChangesAsync();
            }
        }

    }
}