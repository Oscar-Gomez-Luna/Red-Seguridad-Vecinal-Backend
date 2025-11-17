using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Invitados;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitadosController : ControllerBase
    {
        private readonly InvitadosData _invitadosData;

        public InvitadosController(InvitadosData invitadosData)
        {
            _invitadosData = invitadosData;
        }

        [HttpPost]
        public async Task<IActionResult> CrearInvitado([FromBody] CrearInvitadoRequest request)
        {
            var invitado = await _invitadosData.CrearInvitadoAsync(request);
            return Ok(new
            {
                message = "Invitado registrado exitosamente",
                id = invitado.InvitadoID,
                qrCode = invitado.CodigoQR
            });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetInvitadosByUsuario(int usuarioId)
        {
            var invitados = await _invitadosData.GetInvitadosByUsuarioAsync(usuarioId);
            return Ok(invitados);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarInvitacion(int id)
        {
            var resultado = await _invitadosData.CancelarInvitacionAsync(id);
            if (!resultado)
                return NotFound(new { message = "Invitado no encontrado" });

            return Ok(new { message = "Invitación cancelada" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvitados()
        {
            var invitados = await _invitadosData.GetAllInvitadosAsync();
            return Ok(invitados);
        }
    }
}
