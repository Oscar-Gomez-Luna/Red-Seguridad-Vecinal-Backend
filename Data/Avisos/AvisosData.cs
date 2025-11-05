using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_RSV.Data.Avisos
{
    public class AvisosData
    {
        private readonly AppDbContext _context;

        public AvisosData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Aviso>> GetAllAsync()
        {
            return await _context.Avisos
                .Include(a => a.Categoria)
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.FechaPublicacion)
                .ToListAsync();
        }

        public async Task<Aviso?> GetByIdAsync(int id)
        {
            return await _context.Avisos
                .Include(a => a.Categoria)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.AvisoID == id);
        }

        public async Task<Aviso> AddAsync(Aviso aviso)
        {
            _context.Avisos.Add(aviso);
            await _context.SaveChangesAsync();
            return aviso;
        }

        public async Task<Aviso?> UpdateAsync(Aviso aviso)
        {
            var existing = await _context.Avisos.FindAsync(aviso.AvisoID);
            if (existing == null) return null;

            existing.Titulo = aviso.Titulo;
            existing.Descripcion = aviso.Descripcion;
            existing.FechaEvento = aviso.FechaEvento;
            existing.CategoriaID = aviso.CategoriaID;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var aviso = await _context.Avisos.FindAsync(id);
            if (aviso == null) return false;

            _context.Avisos.Remove(aviso);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoriaAviso>> GetAllCatAsync()
        {
            return await _context.CategoriasAviso
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}