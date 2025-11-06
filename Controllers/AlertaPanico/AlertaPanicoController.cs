using Microsoft.AspNetCore.Mvc;
using MiApi.Data;
using Backend_RSV.Data.Alertas;
using Backend_RSV.Services;
using System;
using System.Threading.Tasks;

namespace Backend_RSV.Controllers.Alertas
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertasController : ControllerBase
    {
        private readonly AlertaPanicoData _alertasData;
        private readonly FirebaseNotificationService _firebaseService;

        public AlertasController(AlertaPanicoData alertasData, FirebaseNotificationService firebaseService)
        {
            _alertasData = alertasData;
            _firebaseService = firebaseService;
        }

        // GET: api/alertas
        [HttpGet]
        public async Task<IActionResult> GetAlertas()
        {
            try
            {
                var alertas = await _alertasData.GetAlertasAsync();
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener alertas", error = ex.Message });
            }
        }

        // GET: api/alertas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlerta(int id)
        {
            try
            {
                var alerta = await _alertasData.GetAlertaByIdAsync(id);
                if (alerta == null)
                    return NotFound(new { message = "Alerta no encontrada" });

                return Ok(alerta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener alerta", error = ex.Message });
            }
        }

        // GET: api/alertas/usuario/5
        [HttpGet("usuario/{userId}")]
        public async Task<IActionResult> GetAlertasByUsuario(int userId)
        {
            try
            {
                var alertas = await _alertasData.GetAlertasByUsuarioAsync(userId);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener alertas del usuario", error = ex.Message });
            }
        }

        // POST: api/alertas
        [HttpPost]
        public async Task<IActionResult> CreateAlerta([FromBody] AlertaPanicoRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Datos de alerta inválidos" });

            try
            {
                // 1. Crear alerta en MySQL
                var alerta = await _alertasData.CreateAlertaAsync(request);
                
                // 2. Enviar notificación push a comité y vecinos cercanos
                await _firebaseService.SendPanicAlertNotification(alerta);
                
                return Ok(new { 
                    message = "Alerta creada y notificaciones enviadas", 
                    alertaId = alerta.AlertaID 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error al crear alerta", 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message 
                });
            }
        }
    }
}