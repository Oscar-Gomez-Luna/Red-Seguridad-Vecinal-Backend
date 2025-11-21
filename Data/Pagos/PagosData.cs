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
        private readonly ComprobantePdfService _pdfService;

        public PagosData(AppDbContext context, ComprobantePdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
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
                .Include(c => c.DetallesPago)
                    .ThenInclude(dp => dp.Pago)
                .FirstOrDefaultAsync(c => c.CargoMantenimientoID == id);
        }
        public async Task<List<CargoServicio>> GetByUsuarioIdCargoServicioAsync(int usuarioId)
        {
            return await _context.CargosServicios
                .Include(c => c.Solicitud)
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
        public async Task<ComprobantePago> RegistrarPagoAsync(Pago pago)
        {
            using var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                var cuenta = await _context.CuentaUsuario
                    .FirstOrDefaultAsync(c => c.UsuarioID == pago.UsuarioID);


                pago.UltimosDigitosTarjeta = cuenta?.UltimosDigitos;

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();

                foreach (var d in pago.DetallesPago)
                {
                    await ActualizarCargoAsync(d);
                }

                await _context.SaveChangesAsync();

                var pagoConUsuario = await _context.Pagos
                    .Include(p => p.Usuario).ThenInclude(u => u.Persona)
                    .Include(p => p.DetallesPago).ThenInclude(d => d.CargoMantenimiento)
                    .Include(p => p.DetallesPago).ThenInclude(d => d.CargoServicio)
                    .FirstOrDefaultAsync(p => p.PagoID == pago.PagoID);

                var detallesConNav = await _context.DetallePago
                    .Where(dp => dp.PagoID == pago.PagoID)
                    .Include(dp => dp.CargoMantenimiento)
                    .Include(dp => dp.CargoServicio)
                    .ToListAsync();

                var pdfBytes = _pdfService.GenerarPdfTicket(pagoConUsuario!, detallesConNav);

                var comprobante = new ComprobantePago
                {
                    PagoID = pago.PagoID,
                    Archivo = pdfBytes,
                    NombreArchivo = $"Comprobante_{pago.FolioUnico}.pdf",
                    TipoArchivo = "application/pdf",
                    FechaSubida = DateTime.Now
                };

                _context.ComprobantesPago.Add(comprobante);
                await _context.SaveChangesAsync();

                await trx.CommitAsync();
                return comprobante;
            }
            catch
            {
                await trx.RollbackAsync();
                throw;
            }
        }

        private async Task ActualizarCargoAsync(DetallePago detalle)
        {
            if (detalle.CargoMantenimientoID is not null)
            {
                var cargo = await _context.CargosMantenimiento
                    .FirstOrDefaultAsync(c => c.CargoMantenimientoID == detalle.CargoMantenimientoID);

                if (cargo != null)
                {
                    decimal montoRestante = cargo.Monto - cargo.MontoPagado;

                    if (detalle.MontoAplicado > montoRestante)
                        throw new InvalidOperationException($"El monto a pagar ({detalle.MontoAplicado:C}) supera el saldo pendiente ({montoRestante:C}) del cargo de mantenimiento.");

                    cargo.MontoPagado += detalle.MontoAplicado;
                    cargo.Estado = (cargo.Monto - cargo.MontoPagado) <= 0 ? "Pagado" : "Pendiente";

                    _context.CargosMantenimiento.Update(cargo);
                }
            }

            if (detalle.CargoServicioID is not null)
            {
                var cargo = await _context.CargosServicios
                    .FirstOrDefaultAsync(c => c.CargoServicioID == detalle.CargoServicioID);

                if (cargo != null)
                {
                    decimal montoRestante = cargo.Monto - cargo.MontoPagado;

                    if (detalle.MontoAplicado > montoRestante)
                        throw new InvalidOperationException($"El monto a pagar ({detalle.MontoAplicado:C}) supera el saldo pendiente ({montoRestante:C}) del cargo de servicio.");

                    cargo.MontoPagado += detalle.MontoAplicado;
                    cargo.Estado = (cargo.Monto - cargo.MontoPagado) <= 0 ? "Pagado" : "Pendiente";

                    _context.CargosServicios.Update(cargo);
                }
            }
        }


        public async Task<PagoDto?> ObtenerPagoPorIdAsync(int id)
        {
            return await _context.Pagos
                .Where(p => p.PagoID == id)
                .Select(p => new PagoDto
                {
                    PagoID = p.PagoID,
                    FolioUnico = p.FolioUnico,
                    MontoTotal = p.MontoTotal,
                    TipoPago = p.TipoPago,
                    MetodoPago = p.MetodoPago,
                    FechaPago = p.FechaPago,
                    Comprobante = p.Comprobante == null ? null : new ComprobanteDto
                    {
                        ComprobantePagoID = p.Comprobante.ComprobanteID,
                        NombreArchivo = p.Comprobante.NombreArchivo,
                        TipoArchivo = p.Comprobante.TipoArchivo
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PagoDto>> ObtenerPagosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Pagos
                .Where(p => p.UsuarioID == usuarioId)
                .OrderByDescending(p => p.FechaPago)
                .Select(p => new PagoDto
                {
                    PagoID = p.PagoID,
                    FolioUnico = p.FolioUnico,
                    MontoTotal = p.MontoTotal,
                    TipoPago = p.TipoPago,
                    MetodoPago = p.MetodoPago,
                    FechaPago = p.FechaPago,
                    Comprobante = p.Comprobante == null ? null : new ComprobanteDto
                    {
                        ComprobantePagoID = p.Comprobante.ComprobanteID,
                        NombreArchivo = p.Comprobante.NombreArchivo,
                        TipoArchivo = p.Comprobante.TipoArchivo
                    }
                })
                .ToListAsync();
        }

        public async Task<ComprobantePago?> ObtenerComprobantePorPagoAsync(int pagoId)
        {
            return await _context.ComprobantesPago
                .FirstOrDefaultAsync(c => c.PagoID == pagoId);
        }
    }
}