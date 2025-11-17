using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Amenidades;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly AmenidadesData _amenidadesData;

        public ReservasController(AmenidadesData amenidadesData)
        {
            _amenidadesData = amenidadesData;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] CreateReservaRequest request)
        {
            var reserva = await _amenidadesData.CreateReservaAsync(request);
            return Ok(new { message = "Reserva solicitada exitosamente", id = reserva.ReservaID });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetReservasByUsuario(int usuarioId)
        {
            var reservas = await _amenidadesData.GetReservasByUsuarioAsync(usuarioId);
            return Ok(reservas);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservas()
        {
            var reservas = await _amenidadesData.GetAllReservasAsync();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservaById(int id)
        {
            var reserva = await _amenidadesData.GetReservaByIdAsync(id);
            if (reserva == null)
                return NotFound(new { message = "Reserva no encontrada" });

            return Ok(reserva);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var resultado = await _amenidadesData.CancelarReservaAsync(id);
            if (!resultado)
                return NotFound(new { message = "Reserva no encontrada" });

            return Ok(new { message = "Reserva cancelada exitosamente" });
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstadoReserva(int id, [FromBody] UpdateEstadoReservaRequest request)
        {
            var resultado = await _amenidadesData.UpdateEstadoReservaAsync(id, request.Estado);
            if (!resultado)
                return NotFound(new { message = "Reserva no encontrada" });

            return Ok(new { message = "Estado de reserva actualizado" });
        }
    }
}