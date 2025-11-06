using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_RSV.Data.Alertas
{
    public class AlertaPanicoData
    {
        private readonly AppDbContext _context;

        public AlertaPanicoData(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL - CON DTO
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

        // GET BY ID - CON DTO
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

        // GET BY USUARIO - CON DTO
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

        // CREATE - SE MANTIENE IGUAL
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
    }

    // SOLO las clases Request aquí (NO los DTOs)
    public class AlertaPanicoRequest
    {
        public int UsuarioID { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }

    public class UpdateEstadoRequest
    {
        public string Estado { get; set; } = string.Empty;
    }
}