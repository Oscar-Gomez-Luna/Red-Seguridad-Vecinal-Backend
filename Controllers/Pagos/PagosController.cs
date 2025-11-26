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

        [HttpGet("cargos/mantenimiento")]
        public async Task<IActionResult> GetAllCargosMantenimiento()
        {
            var cargos = await _pagosData.GetAllCargosMantenimientoAsync();

            var result = cargos.Select(c => new
            {
                cargoMantenimientoID = c.CargoMantenimientoID,
                usuarioId = c.UsuarioID,
                usuarioNombre = c.Usuario?.Persona.Nombre,
                usuarioApellidoP = c.Usuario?.Persona.ApellidoPaterno,
                usuarioApellidoM = c.Usuario?.Persona.ApellidoMaterno,
                concepto = c.Concepto,
                monto = c.Monto,
                montoPagado = c.MontoPagado,
                saldoPendiente = c.SaldoPendiente,
                estado = c.Estado,
                fechaVencimiento = c.FechaVencimiento,
                fechaCreacion = c.FechaCreacion
            });

            return Ok(result);
        }


        [HttpGet("cargos/servicio")]
        public async Task<IActionResult> GetAllCargosServicios()
        {
            var cargos = await _pagosData.GetAllCargosServiciosAsync();

            var result = cargos.Select(c => new
            {
                cargoServicioID = c.CargoServicioID,

                usuarioId = c.Solicitud.Usuario.UsuarioID,
                usuarioNombre = c.Solicitud.Usuario.Persona.Nombre,
                usuarioApellidoP = c.Solicitud.Usuario.Persona.ApellidoPaterno,
                usuarioApellidoM = c.Solicitud.Usuario.Persona.ApellidoMaterno,
                concepto = c.Concepto,
                monto = c.Monto,
                montoPagado = c.MontoPagado,
                saldoPendiente = c.SaldoPendiente,
                estado = c.Estado,
                fechaCreacion = c.FechaCreacion,

                solicitud = new
                {
                    c.Solicitud.SolicitudID,
                    c.Solicitud.Descripcion
                }
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPago([FromBody] PagoRegistroRequest request)
        {
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
                FolioUnico = Guid.NewGuid().ToString("N")[..12].ToUpper(),
                MontoTotal = request.MontoTotal,
                TipoPago = request.TipoPago,
                MetodoPago = request.MetodoPago,
                FechaPago = DateTime.Now
            };

            foreach (var d in detallesRequest)
            {
                pago.DetallesPago.Add(new DetallePago
                {
                    MontoAplicado = d.MontoAplicado,
                    CargoMantenimientoID = d.CargoMantenimientoID,
                    CargoServicioID = d.CargoServicioID,
                    FechaAplicacion = DateTime.Now
                });
            }

            try
            {
                var nuevoPago = await _pagosData.RegistrarPagoAsync(pago);

                return File(
                    fileContents: nuevoPago.Archivo,
                    contentType: nuevoPago.TipoArchivo,
                    fileDownloadName: nuevoPago.NombreArchivo
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar el pago.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPago(int id)
        {
            var pago = await _pagosData.ObtenerPagoPorIdAsync(id);
            if (pago == null)
                return NotFound(new { message = "Pago no encontrado" });

            return Ok(pago);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodosLosPagos()
        {
            var pagos = await _pagosData.ObtenerTodosLosPagosAsync();
            return Ok(pagos);
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
        [HttpGet("comprobantes")]
        public async Task<IActionResult> ObtenerTodosLosComprobantes()
        {
            var comprobantes = await _pagosData.ObtenerTodosLosComprobantesAsync();

            var result = comprobantes.Select(c => new
            {
                c.ComprobanteID,
                c.PagoID,
                c.NombreArchivo,
                c.TipoArchivo,
                Tamaño = c.Archivo?.Length
            });

            return Ok(result);
        }

    }
}