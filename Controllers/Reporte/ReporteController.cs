using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Reportes;
using System.Threading.Tasks;
using Backend_RSV.Models.Request;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly ReporteData _reporteData;

        public ReportesController(ReporteData reporteData)
        {
            _reporteData = reporteData;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportes()
        {
            var reportes = await _reporteData.GetReportesAsync();
            return Ok(reportes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReporteById(int id)
        {
            var reporte = await _reporteData.GetReporteByIdAsync(id);
            if (reporte == null)
                return NotFound(new { message = "Reporte no encontrado" });

            return Ok(reporte);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetReportesByUsuario(int usuarioId)
        {
            var reportes = await _reporteData.GetReportesByUsuarioAsync(usuarioId);
            return Ok(reportes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReporte([FromBody] ReporteRequest request)
        {
            var reporte = await _reporteData.CreateReporteAsync(request);
            return Ok(new { message = "Reporte creado exitosamente", id = reporte.ReporteID });
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateReporteEstado(int id, [FromBody] UpdateEstadoReporteRequest request)
        {
            var reporte = await _reporteData.UpdateReporteEstadoAsync(id, request.Visto);
            if (reporte == null)
                return NotFound(new { message = "Reporte no encontrado" });

            return Ok(new { message = "Estado del reporte actualizado" });
        }

        [HttpPut("{id}/anonimato")]
        public async Task<IActionResult> CambiarAnonimato(int id, [FromBody] CambiarAnonimatoRequest request)
        {
            var reporte = await _reporteData.CambiarAnonimatoAsync(id, request.EsAnonimo);
            if (reporte == null)
                return NotFound(new { message = "Reporte no encontrado" });

            return Ok(new { message = "Anonimato del reporte actualizado" });
        }

        [HttpGet("tipos-reporte")]
        public async Task<IActionResult> GetTiposReporte()
        {
            var tipos = await _reporteData.GetTiposReporteAsync();
            return Ok(tipos);
        }
    }
}