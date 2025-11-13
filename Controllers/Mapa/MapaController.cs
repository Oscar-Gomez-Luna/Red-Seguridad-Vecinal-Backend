using Backend_RSV.Data.Mapa;
using Backend_RSV.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Backend_RSV.Controllers.Mapa
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapaController : ControllerBase
    {
        private readonly MapaData _mapaData;

        public MapaController(MapaData mapaData)
        {
            _mapaData = mapaData;
        }

        [HttpGet("marcadores")]
        public async Task<IActionResult> GetMarcadores()
        {
            var marcadores = await _mapaData.ObtenerMarcadoresActivosAsync();
            return Ok(marcadores);
        }

        [HttpGet("marcadores/{id}")]
        public async Task<IActionResult> GetMarcador(int id)
        {
            var marcador = await _mapaData.ObtenerMarcadorPorIdAsync(id);
            if (marcador == null)
                return NotFound(new { message = "Marcador no encontrado." });

            return Ok(marcador);
        }

        [HttpPost("marcadores")]
        public async Task<IActionResult> CrearMarcador([FromBody] MarcadorMapaRegisterRequest request)
        {
            var marcador = new MarcadorMapa
            {
                UsuarioID = request.UsuarioID,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                Indicador = request.Indicador,
                Comentario = request.Comentario,
                FechaCreacion = DateTime.Now
            };

            var nuevo = await _mapaData.CrearMarcadorAsync(marcador);
            return Ok(nuevo);
        }

        [HttpPut("marcadores")]
        public async Task<IActionResult> ActualizarMarcador([FromBody] MarcadorMapaUpdateRequest request)
        {
            var marcador = new MarcadorMapa
            {
                MarcadorID = request.MarcadorID,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                Indicador = request.Indicador,
                Comentario = request.Comentario
            };

            var actualizado = await _mapaData.ActualizarMarcadorAsync(marcador);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("marcadores/{id}")]
        public async Task<IActionResult> EliminarMarcador(int id)
        {
            var eliminado = await _mapaData.EliminarMarcadorAsync(id);
            if (!eliminado)
                return NotFound(new { message = "Marcador no encontrado." });

            return Ok(new { message = "Marcador eliminado correctamente (borrado lógico)." });
        }
    }

}