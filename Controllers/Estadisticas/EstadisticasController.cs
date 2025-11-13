using System.Globalization;
using Backend_RSV.Data.Estadisticas;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Backend_RSV.Controllers.Estadisticas
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadisticasController : ControllerBase
    {
        private readonly EstadisticasData _estadisticasData;

        public EstadisticasController(EstadisticasData estadisticasData)
        {
            _estadisticasData = estadisticasData;
        }

        [HttpGet("incidentes")]
        public async Task<IActionResult> GetEstadisticasIncidentes()
        {
            try
            {
                var datos = await _estadisticasData.ObtenerEstadisticasIncidentesAsync();
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener estadísticas de incidentes.", error = ex.Message });
            }
        }

        [HttpGet("pagos")]
        public async Task<IActionResult> GetEstadisticasPagos()
        {
            try
            {
                var datos = await _estadisticasData.ObtenerEstadisticasPagosAsync();
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener estadísticas de pagos.", error = ex.Message });
            }
        }

        [HttpGet("servicios")]
        public async Task<IActionResult> GetEstadisticasServicios()
        {
            try
            {
                var datos = await _estadisticasData.ObtenerEstadisticasServiciosAsync();
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener estadísticas de servicios.", error = ex.Message });
            }
        }
        [HttpGet("exportar")]
        public async Task<IActionResult> ExportarEstadisticas([FromQuery] string tipo = "pdf")
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var datos = await _estadisticasData.ObtenerEstadisticasGeneralesAsync();
            var opcionesJson = new JsonSerializerOptions { WriteIndented = true };
            if (tipo.Equals("pdf", StringComparison.OrdinalIgnoreCase))
            {
                var stream = new MemoryStream();

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Header()
                            .Text("Reporte General de Estadísticas")
                            .FontSize(20)
                            .Bold()
                            .AlignCenter();

                        page.Content().PaddingVertical(10).Column(col =>
                        {
                            col.Item().Text("Estadísticas de Incidentes").FontSize(16).Bold();
                            col.Item().Text(JsonSerializer.Serialize(datos?.Incidentes, opcionesJson))
                                .FontSize(10);

                            col.Spacing(10);
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().Text("Estadísticas de Pagos").FontSize(16).Bold();
                            col.Item().Text(JsonSerializer.Serialize(datos?.Pagos, opcionesJson))
                                .FontSize(10);

                            col.Spacing(10);
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().Text("Estadísticas de Servicios").FontSize(16).Bold();
                            col.Item().Text(JsonSerializer.Serialize(datos?.Servicios, opcionesJson))
                                .FontSize(10);
                        });

                        page.Footer()
                            .AlignRight()
                            .Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                }).GeneratePdf(stream);

                stream.Position = 0;
                return File(stream, "application/pdf", "estadisticas.pdf");
            }
            else if (tipo.Equals("excel", StringComparison.OrdinalIgnoreCase))
            {
                using var workbook = new XLWorkbook();

                var hoja1 = workbook.Worksheets.Add("Incidentes");
                hoja1.Cell(1, 1).Value = "Estadísticas de Incidentes";
                hoja1.Cell(2, 1).Value = JsonSerializer.Serialize(datos?.Incidentes, opcionesJson);
                hoja1.Columns().AdjustToContents();

                var hoja2 = workbook.Worksheets.Add("Pagos");
                hoja2.Cell(1, 1).Value = "Estadísticas de Pagos";
                hoja2.Cell(2, 1).Value = JsonSerializer.Serialize(datos?.Pagos, opcionesJson);
                hoja2.Columns().AdjustToContents();

                var hoja3 = workbook.Worksheets.Add("Servicios");
                hoja3.Cell(1, 1).Value = "Estadísticas de Servicios";
                hoja3.Cell(2, 1).Value = JsonSerializer.Serialize(datos?.Servicios, opcionesJson);
                hoja3.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "estadisticas.xlsx"
                );
            }

            return BadRequest(new { message = "Tipo de exportación no válido. Usa 'pdf' o 'excel'." });
        }

    }
}