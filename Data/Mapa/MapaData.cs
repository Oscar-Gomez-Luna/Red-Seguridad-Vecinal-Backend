using MiApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_RSV.Data.Mapa
{
    public class MapaData
    {
        private readonly AppDbContext _context;

        public MapaData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MarcadorMapa>> ObtenerMarcadoresActivosAsync()
        {
            return await _context.MarcadoresMapa
                .Include(m => m.Usuario)
                .Where(m => m.Activo)
                .OrderByDescending(m => m.FechaCreacion)
                .ToListAsync();
        }

        public async Task<MarcadorMapa?> ObtenerMarcadorPorIdAsync(int id)
        {
            return await _context.MarcadoresMapa
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MarcadorID == id);
        }

        public async Task<MarcadorMapa> CrearMarcadorAsync(MarcadorMapa marcador)
        {
            _context.MarcadoresMapa.Add(marcador);
            await _context.SaveChangesAsync();
            return marcador;
        }

        public async Task<bool> ActualizarMarcadorAsync(MarcadorMapa marcador)
        {
            var existente = await _context.MarcadoresMapa.FindAsync(marcador.MarcadorID);
            if (existente == null)
                return false;

            existente.Latitud = marcador.Latitud;
            existente.Longitud = marcador.Longitud;
            existente.Indicador = marcador.Indicador;
            existente.Comentario = marcador.Comentario;
            existente.Activo = marcador.Activo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarMarcadorAsync(int id)
        {
            var marcador = await _context.MarcadoresMapa.FindAsync(id);
            if (marcador == null)
                return false;

            marcador.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}