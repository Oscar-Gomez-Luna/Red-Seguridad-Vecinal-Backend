using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_RSV.Data.Avisos;
using Microsoft.AspNetCore.Mvc;

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
            var result = avisos.Select(a => new
            {
                a.AvisoID,
                a.Titulo,
                a.Descripcion,
                a.FechaEvento,
                a.FechaPublicacion,
                Usuario = new { a.Usuario.UsuarioID, a.Usuario.Persona.Nombre },
                Categoria = new { a.Categoria.CategoriaID, a.Categoria.Nombre }
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAviso(int id)
        {
            var aviso = await _avisosData.GetByIdAsync(id);
            if (aviso == null) return NotFound(new { message = "Aviso no encontrado." });

            return Ok(new
            {
                aviso.AvisoID,
                aviso.Titulo,
                aviso.Descripcion,
                aviso.FechaEvento,
                aviso.FechaPublicacion,
                Usuario = new { aviso.Usuario.UsuarioID, aviso.Usuario.Persona.Nombre },
                Categoria = new { aviso.Categoria.CategoriaID, aviso.Categoria.Nombre }
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAviso([FromBody] Aviso aviso)
        {
            var nuevoAviso = await _avisosData.AddAsync(aviso);
            return CreatedAtAction(nameof(GetAviso), new { id = nuevoAviso.AvisoID }, nuevoAviso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAviso(int id, [FromBody] Aviso aviso)
        {
            if (id != aviso.AvisoID) return BadRequest();

            var actualizado = await _avisosData.UpdateAsync(aviso);
            if (actualizado == null) return NotFound(new { message = "Aviso no encontrado." });

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