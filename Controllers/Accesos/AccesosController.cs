using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Invitados;
using System.Threading.Tasks;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesosController : ControllerBase
    {
        private readonly InvitadosData _invitadosData;

        public AccesosController(InvitadosData invitadosData)
        {
            _invitadosData = invitadosData;
        }

        // 🟦 PWA: Validar QR (entrada/salida)
        [HttpGet("validar/{codigoQR}")]
        public async Task<IActionResult> ValidarQR(string codigoQR)
        {
            var resultado = await _invitadosData.ValidarQRAsync(codigoQR);
            
            if (!resultado.Exitoso)
                return BadRequest(new { message = resultado.Mensaje });
            
            return Ok(new { 
                message = resultado.Mensaje,
                tipo = resultado.Tipo,
                nombre = resultado.Nombre,
                esEntrada = resultado.EsEntrada
            });
        }

        // 🟦 PWA: Historial de accesos
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorialAccesos()
        {
            var historial = await _invitadosData.GetHistorialAccesosAsync();
            return Ok(historial);
        }

        // 🟩 AMBAS: Historial de accesos del usuario
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetAccesosByUsuario(int usuarioId)
        {
            var accesos = await _invitadosData.GetAccesosByUsuarioAsync(usuarioId);
            return Ok(accesos);
        }
    }
}