using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Invitados;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRPersonalController : ControllerBase
    {
        private readonly InvitadosData _invitadosData;

        public QRPersonalController(InvitadosData invitadosData)
        {
            _invitadosData = invitadosData;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> GenerarQRPersonal([FromBody] GenerarQRRequest request)
        {
            var qrPersonal = await _invitadosData.GenerarQRPersonalAsync(request.UsuarioID);
            return Ok(new
            {
                message = "QR personal generado exitosamente",
                id = qrPersonal.QRID,
                qrCode = qrPersonal.CodigoQR,
                fechaVencimiento = qrPersonal.FechaVencimiento
            });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetQRPersonalByUsuario(int usuarioId)
        {
            var qrPersonal = await _invitadosData.GetQRPersonalByUsuarioAsync(usuarioId);
            if (qrPersonal == null)
                return NotFound(new { message = "QR personal no encontrado" });

            return Ok(qrPersonal);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstadoQR(int id, [FromBody] UpdateEstadoQRRequest request)
        {
            var resultado = await _invitadosData.UpdateEstadoQRAsync(id, request.Activo);
            if (!resultado)
                return NotFound(new { message = "QR personal no encontrado" });

            return Ok(new { message = "Estado del QR actualizado" });
        }
    }
}