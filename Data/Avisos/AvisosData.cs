using Backend_RSV.Models.Request;
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

        public async Task<List<AvisosDTO>> GetAllAsync()
        {
            return await _context.Avisos
                .Include(a => a.Categoria)
                .OrderByDescending(a => a.FechaPublicacion)
                .Select(a => new AvisosDTO
                {
                    AvisoID = a.AvisoID,
                    UsuarioID = a.UsuarioID,
                    CategoriaID = a.CategoriaID,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    FechaEvento = a.FechaEvento,
                    FechaPublicacion = a.FechaPublicacion,
                    CategoriaNombre = a.Categoria.Nombre,
                    CategoriaActiva = a.Categoria.Activo,

                })
                .ToListAsync();
        }
        public async Task<AvisosDTO?> GetByIdAsync(int id)
        {
            return await _context.Avisos
                .Include(a => a.Categoria)
                .Where(a => a.AvisoID == id)
                .Select(a => new AvisosDTO
                {
                    AvisoID = a.AvisoID,
                    UsuarioID = a.UsuarioID,
                    CategoriaID = a.CategoriaID,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    FechaEvento = a.FechaEvento,
                    FechaPublicacion = a.FechaPublicacion,

                    CategoriaNombre = a.Categoria.Nombre,
                    CategoriaActiva = a.Categoria.Activo,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Aviso> AddAsync(AvisoRegistroRequest request)
        {
            var aviso = new Aviso
            {
                UsuarioID = request.UsuarioID,
                CategoriaID = request.CategoriaID,
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                FechaEvento = request.FechaEvento,
                FechaPublicacion = DateTime.Now
            };

            _context.Avisos.Add(aviso);
            await _context.SaveChangesAsync();
            return aviso;
        }

        public async Task<Aviso?> UpdateAsync(int id, AvisoUpdateRequest request)
        {
            var aviso = await _context.Avisos.FindAsync(id);
            if (aviso == null)
                return null;

            aviso.CategoriaID = request.CategoriaID;
            aviso.Titulo = request.Titulo;
            aviso.Descripcion = request.Descripcion;
            aviso.FechaEvento = request.FechaEvento;

            await _context.SaveChangesAsync();
            return aviso;
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