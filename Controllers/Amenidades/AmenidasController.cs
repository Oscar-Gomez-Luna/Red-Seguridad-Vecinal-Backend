using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Amenidades;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmenidadesController : ControllerBase
    {
        private readonly AmenidadesData _amenidadesData;

        public AmenidadesController(AmenidadesData amenidadesData)
        {
            _amenidadesData = amenidadesData;
        }

        [HttpGet]
        public async Task<IActionResult> GetAmenidades()
        {
            var amenidades = await _amenidadesData.GetAmenidadesAsync();
            return Ok(amenidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenidadById(int id)
        {
            var amenidad = await _amenidadesData.GetAmenidadByIdAsync(id);
            if (amenidad == null)
                return NotFound(new { message = "Amenidad no encontrada" });

            return Ok(amenidad);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmenidad([FromBody] CreateAmenidadRequest request)
        {
            var amenidad = await _amenidadesData.CreateAmenidadAsync(request);
            return Ok(new { message = "Amenidad creada exitosamente", id = amenidad.AmenidadID });
        }

        [HttpGet("tipos-amenidad")]
        public async Task<IActionResult> GetTiposAmenidad()
        {
            var tipos = await _amenidadesData.GetTiposAmenidadAsync();
            return Ok(tipos);
        }
    }
}