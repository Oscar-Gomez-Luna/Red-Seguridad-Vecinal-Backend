using Backend_RSV.Data.Avisos;
using Backend_RSV.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_RSV.Controllers.Avisos
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvisosController : ControllerBase
    {
        private readonly AvisosData _avisosData;

        public AvisosController(AvisosData avisosData)
        {
            _avisosData = avisosData;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvisos()
        {
            var avisos = await _avisosData.GetAllAsync();
            return Ok(avisos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAviso(int id)
        {
            var aviso = await _avisosData.GetByIdAsync(id);

            if (aviso == null)
                return NotFound(new { message = "Aviso no encontrado." });

            return Ok(aviso);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAviso([FromBody] AvisoRegistroRequest aviso)
        {
            try
            {
                if (aviso == null)
                    return BadRequest(new { mensaje = "El cuerpo de la solicitud es inválido." });

                if (string.IsNullOrWhiteSpace(aviso.Titulo))
                    return BadRequest(new { mensaje = "El título es obligatorio." });

                if (string.IsNullOrWhiteSpace(aviso.Descripcion))
                    return BadRequest(new { mensaje = "La descripción es obligatoria." });

                if (aviso.UsuarioID <= 0)
                    return BadRequest(new { mensaje = "El UsuarioID es inválido." });

                if (aviso.CategoriaID <= 0)
                    return BadRequest(new { mensaje = "El CategoriaID es inválido." });

                var nuevoAviso = await _avisosData.AddAsync(aviso);

                var response = new
                {
                    nuevoAviso.AvisoID,
                    aviso.UsuarioID,
                    aviso.CategoriaID,
                    aviso.Titulo,
                    aviso.Descripcion,
                    aviso.FechaEvento
                };

                return Ok(response);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al guardar el aviso en la base de datos.",
                    detalle = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error inesperado.",
                    detalle = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAviso([FromBody] AvisoUpdateRequest aviso)
        {
            var actualizado = await _avisosData.UpdateAsync(aviso.AvisoID, aviso);
            if (actualizado == null)
                return NotFound();

            return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAviso(int id)
        {
            var eliminado = await _avisosData.DeleteAsync(id);
            if (!eliminado) return NotFound(new { message = "Aviso no encontrado." });

            return NoContent();
        }

        [HttpGet("categorias-aviso")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _avisosData.GetAllCatAsync();

            var result = categorias.Select(c => new
            {
                c.CategoriaID,
                c.Nombre
            });

            return Ok(result);
        }
    }
}