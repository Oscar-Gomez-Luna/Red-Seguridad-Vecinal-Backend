using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_RSV.Controllers.Pagos
{
    public class PagosData
    {
        private readonly AppDbContext _context;

        public PagosData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CuentaUsuario?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.CuentaUsuario
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuarioID == usuarioId);
        }

        public async Task<List<CargoMantenimiento>> GetCargosMantenimientoAsync(int usuarioId)
        {
            return await _context.CargosMantenimiento
                .Where(c => c.UsuarioID == usuarioId)
                .OrderBy(c => c.FechaVencimiento)
                .ToListAsync();
        }

        public async Task<List<CargoServicio>> GetCargosServiciosAsync(int usuarioId)
        {
            return await _context.CargosServicios
                .Where(c => c.UsuarioID == usuarioId)
                .OrderBy(c => c.FechaCreacion)
                .ToListAsync();
        }
        public async Task<List<CargoMantenimiento>> GetByUsuarioIdCargoMantenimientoAsync(int usuarioId)
        {
            return await _context.CargosMantenimiento
                .Where(c => c.UsuarioID == usuarioId)
                .OrderBy(c => c.FechaVencimiento)
                .ToListAsync();
        }

        public async Task<CargoMantenimiento?> GetByCargoMantenimientoIdAsync(int id)
        {
            return await _context.CargosMantenimiento
                .Include(c => c.Usuario)
                .Include(c => c.Pagos)
                .FirstOrDefaultAsync(c => c.CargoMantenimientoID == id);
        }
        public async Task<List<CargoServicio>> GetByUsuarioIdCargoServicioAsync(int usuarioId)
        {
            return await _context.CargosServicios
                .Include(c => c.Solicitud) // Incluye información de la solicitud
                .OrderBy(c => c.FechaCreacion)
                .Where(c => c.UsuarioID == usuarioId)
                .ToListAsync();
        }

        public async Task<CargoServicio?> GetByIdCargoServicioAsync(int id)
        {
            return await _context.CargosServicios
                .Include(c => c.Usuario)
                .Include(c => c.Solicitud)
                .FirstOrDefaultAsync(c => c.CargoServicioID == id);
        }
        // 1️⃣ Registrar nuevo pago
        public async Task<Pago> RegistrarPagoAsync(Pago pago, byte[]? archivoComprobante = null, string? nombreArchivo = null, string? tipoMime = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();

                if (archivoComprobante != null)
                {
                    var comprobante = new ComprobantePago
                    {
                        PagoID = pago.PagoID,
                        Archivo = archivoComprobante,
                        NombreArchivo = nombreArchivo ?? $"comprobante_{pago.PagoID}.pdf",
                        TipoArchivo = tipoMime ?? "application/pdf"
                    };

                    _context.ComprobantesPago.Add(comprobante);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return pago;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Pago?> ObtenerPagoPorIdAsync(int id)
        {
            return await _context.Pagos
                .Include(p => p.Usuario)
                .Include(p => p.CargoServicio)
                .Include(p => p.Comprobante)
                .FirstOrDefaultAsync(p => p.PagoID == id);
        }

        public async Task<IEnumerable<Pago>> ObtenerPagosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Pagos
                .Where(p => p.UsuarioID == usuarioId)
                .Include(p => p.Comprobante)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<ComprobantePago?> ObtenerComprobantePorPagoAsync(int pagoId)
        {
            return await _context.ComprobantesPago
                .FirstOrDefaultAsync(c => c.PagoID == pagoId);
        }
    }
}