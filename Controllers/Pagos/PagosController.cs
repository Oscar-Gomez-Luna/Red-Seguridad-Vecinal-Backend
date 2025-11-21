using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Backend_RSV.Controllers.Pagos
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly PagosData _pagosData;

        public PagosController(PagosData pagosData)
        {
            _pagosData = pagosData;
        }
        [HttpGet("cuenta/{usuarioId}")]
        public async Task<IActionResult> GetCuenta(int usuarioId)
        {
            var cuenta = await _pagosData.GetByUsuarioIdAsync(usuarioId);
            if (cuenta == null) return NotFound(new { message = "Cuenta no encontrada." });

            var cargosMantenimiento = await _pagosData.GetCargosMantenimientoAsync(usuarioId);
            var cargosServicios = await _pagosData.GetCargosServiciosAsync(usuarioId);

            return Ok(new
            {
                CuentaID = cuenta.CuentaID,
                SaldoMantenimiento = cuenta.SaldoMantenimiento,
                SaldoServicios = cuenta.SaldoServicios,
                SaldoTotal = cuenta.SaldoTotal,
                UltimaActualizacion = cuenta.UltimaActualizacion,
                CargosMantenimiento = cargosMantenimiento.Select(c => new
                {
                    c.CargoMantenimientoID,
                    c.Concepto,
                    c.Monto,
                    c.SaldoPendiente,
                    c.Estado,
                    c.FechaVencimiento
                }),
                CargosServicios = cargosServicios.Select(c => new
                {
                    c.CargoServicioID,
                    c.Concepto,
                    c.Monto,
                    c.SaldoPendiente,
                    c.Estado,
                    c.FechaCreacion
                })
            });
        }
        [HttpGet("cargos/mantenimiento/{usuarioId}")]
        public async Task<IActionResult> GetCargosMantenimiento(int usuarioId)
        {
            var cargos = await _pagosData.GetByUsuarioIdCargoMantenimientoAsync(usuarioId);

            var result = cargos.Select(c => new
            {
                c.CargoMantenimientoID,
                c.Concepto,
                c.Monto,
                c.MontoPagado,
                c.SaldoPendiente,
                c.Estado,
                c.FechaVencimiento,
                c.FechaCreacion
            });

            return Ok(result);
        }
        [HttpGet("cargos/servicio/{usuarioId}")]
        public async Task<IActionResult> GetCargosServicios(int usuarioId)
        {
            var cargos = await _pagosData.GetByUsuarioIdCargoServicioAsync(usuarioId);

            var result = cargos.Select(c => new
            {
                c.CargoServicioID,
                c.Concepto,
                c.Monto,
                c.MontoPagado,
                c.SaldoPendiente,
                c.Estado,
                c.FechaCreacion,
                Solicitud = new
                {
                    c.Solicitud.SolicitudID,
                    c.Solicitud.Descripcion
                }
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPago([FromForm] PagoRegistroRequest request, IFormFile? comprobante)
        {
            ComprobantePago? comprobantePago = null;

            if (comprobante != null)
            {
                using var ms = new MemoryStream();
                await comprobante.CopyToAsync(ms);
                comprobantePago = new ComprobantePago
                {
                    Archivo = ms.ToArray(),
                    NombreArchivo = comprobante.FileName,
                    TipoArchivo = comprobante.ContentType
                };
            }

            List<DetallePagoRequest> detallesRequest = new();

            if (!string.IsNullOrWhiteSpace(request.DetallesPagoJson))
            {
                detallesRequest = System.Text.Json.JsonSerializer
                    .Deserialize<List<DetallePagoRequest>>(request.DetallesPagoJson)
                    ?? new List<DetallePagoRequest>();
            }

            var pago = new Pago
            {
                UsuarioID = request.UsuarioID,
                CargoMantenimientoID = request.CargoMantenimientoID,
                CargoServicioID = request.CargoServicioID,
                FolioUnico = request.FolioUnico,
                MontoTotal = request.MontoTotal,
                TipoPago = request.TipoPago,
                MetodoPago = request.MetodoPago,
                UltimosDigitosTarjeta = request.UltimosDigitosTarjeta,
                FechaPago = DateTime.Now
            };
            foreach (var d in detallesRequest)
            {
                pago.DetallesPago.Add(new DetallePago
                {
                    TipoCargo = d.TipoCargo,
                    CargoID = d.CargoID,
                    MontoAplicado = d.MontoAplicado,
                    FechaAplicacion = DateTime.Now
                });
            }
            var nuevoPago = await _pagosData.RegistrarPagoAsync(pago, comprobantePago);

            return Ok(new
            {
                message = "Pago registrado correctamente",
                pagoId = nuevoPago.PagoID
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPago(int id)
        {
            var pago = await _pagosData.ObtenerPagoPorIdAsync(id);
            if (pago == null)
                return NotFound(new { message = "Pago no encontrado" });

            return Ok(pago);
        }

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> ObtenerPagosPorUsuario(int id)
        {
            var pagos = await _pagosData.ObtenerPagosPorUsuarioAsync(id);
            return Ok(pagos);
        }

        [HttpGet("comprobante/{id}")]
        public async Task<IActionResult> ObtenerComprobante(int id)
        {
            var comprobante = await _pagosData.ObtenerComprobantePorPagoAsync(id);

            if (comprobante == null)
                return NotFound(new { message = "Comprobante no encontrado" });

            return File(comprobante.Archivo, comprobante.TipoArchivo, comprobante.NombreArchivo);
        }
    }
}