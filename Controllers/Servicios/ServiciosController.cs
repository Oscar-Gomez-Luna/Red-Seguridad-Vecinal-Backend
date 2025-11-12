using Microsoft.AspNetCore.Mvc;
using Backend_RSV.Data.Servicios;
using System.Threading.Tasks;

namespace Backend_RSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly ServiciosData _serviciosData;

        public ServiciosController(ServiciosData serviciosData)
        {
            _serviciosData = serviciosData;
        }

        // ==========================================
        // 1. TIPOS DE SERVICIO - 🟩 AMBAS (2 APIs)
        // ==========================================

        [HttpGet("tipos-servicio")]
        public async Task<IActionResult> GetTiposServicio()
        {
            var tipos = await _serviciosData.GetTiposServicioAsync();
            return Ok(tipos);
        }

        [HttpGet("tipos-servicio/{id}")]
        public async Task<IActionResult> GetTipoServicioById(int id)
        {
            var tipo = await _serviciosData.GetTipoServicioByIdAsync(id);
            if (tipo == null)
                return NotFound(new { message = "Tipo de servicio no encontrado" });
            
            return Ok(tipo);
        }

        // ==========================================
        // 2. CATÁLOGO DE SERVICIOS - 🟩 AMBAS (4 APIs)
        // ==========================================

        [HttpGet("catalogo")]
        public async Task<IActionResult> GetServiciosCatalogo()
        {
            var servicios = await _serviciosData.GetServiciosCatalogoAsync();
            return Ok(servicios);
        }

        [HttpGet("catalogo/{id}")]
        public async Task<IActionResult> GetServicioCatalogoById(int id)
        {
            var servicio = await _serviciosData.GetServicioCatalogoByIdAsync(id);
            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado" });
            
            return Ok(servicio);
        }

        [HttpPost("catalogo")]
        public async Task<IActionResult> CreateServicioCatalogo([FromBody] ServicioCatalogoRequest request)
        {
            var servicio = await _serviciosData.CreateServicioCatalogoAsync(request);
            return Ok(new { message = "Servicio creado exitosamente", id = servicio.ServicioID });
        }

        [HttpPut("catalogo/{id}")]
        public async Task<IActionResult> UpdateServicioCatalogo(int id, [FromBody] ServicioCatalogoRequest request)
        {
            var servicio = await _serviciosData.UpdateServicioCatalogoAsync(id, request);
            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado" });
            
            return Ok(new { message = "Servicio actualizado exitosamente" });
        }

        [HttpPut("catalogo/{id}/disponibilidad")]
        public async Task<IActionResult> UpdateDisponibilidadServicio(int id, [FromBody] UpdateDisponibilidadRequest request)
        {
            var servicio = await _serviciosData.UpdateDisponibilidadServicioAsync(id, request.Disponible);
            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado" });
            
            return Ok(new { message = "Disponibilidad actualizada" });
        }

        // ==========================================
        // 3. PERSONAL MANTENIMIENTO - 🟦 PWA (3 APIs)
        // ==========================================

        [HttpGet("personal-mantenimiento")]
        public async Task<IActionResult> GetPersonalMantenimiento()
        {
            var personal = await _serviciosData.GetPersonalMantenimientoAsync();
            return Ok(personal);
        }

        [HttpGet("personal-mantenimiento/{id}")]
        public async Task<IActionResult> GetPersonalMantenimientoById(int id)
        {
            var personal = await _serviciosData.GetPersonalMantenimientoByIdAsync(id);
            if (personal == null)
                return NotFound(new { message = "Personal no encontrado" });
            
            return Ok(personal);
        }

        [HttpPost("personal-mantenimiento")]
        public async Task<IActionResult> CreatePersonalMantenimiento([FromBody] PersonalMantenimientoRequest request)
        {
            var personal = await _serviciosData.CreatePersonalMantenimientoAsync(request);
            return Ok(new { message = "Personal registrado exitosamente", id = personal.PersonalMantenimientoID });
        }

        // ==========================================
        // 4. SOLICITUDES DE SERVICIO - 🟩 AMBAS (5 APIs)
        // ==========================================

        [HttpPost("solicitud")]
        public async Task<IActionResult> CreateSolicitud([FromBody] SolicitudServicioRequest request)
        {
            var solicitud = await _serviciosData.CreateSolicitudAsync(request);
            return Ok(new { message = "Solicitud creada exitosamente", id = solicitud.SolicitudID });
        }

        [HttpGet("solicitudes")]
        public async Task<IActionResult> GetSolicitudes()
        {
            var solicitudes = await _serviciosData.GetSolicitudesAsync();
            return Ok(solicitudes);
        }

        [HttpGet("solicitud/{id}")]
        public async Task<IActionResult> GetSolicitudById(int id)
        {
            var solicitud = await _serviciosData.GetSolicitudByIdAsync(id);
            if (solicitud == null)
                return NotFound(new { message = "Solicitud no encontrada" });
            
            return Ok(solicitud);
        }

        [HttpGet("solicitud/usuario/{usuarioId}")]
        public async Task<IActionResult> GetSolicitudesByUsuario(int usuarioId)
        {
            var solicitudes = await _serviciosData.GetSolicitudesByUsuarioAsync(usuarioId);
            return Ok(solicitudes);
        }

        [HttpPut("solicitud/{id}/asignar")]
        public async Task<IActionResult> AsignarSolicitud(int id, [FromBody] AsignarSolicitudRequest request)
        {
            var solicitud = await _serviciosData.AsignarSolicitudAsync(id, request.PersonaAsignado);
            if (solicitud == null)
                return NotFound(new { message = "Solicitud no encontrada" });
            
            return Ok(new { message = "Solicitud asignada correctamente" });
        }

        [HttpPut("solicitud/{id}/estado")]
        public async Task<IActionResult> UpdateEstadoSolicitud(int id, [FromBody] UpdateEstadoSolicitudRequest request)
        {
            var solicitud = await _serviciosData.UpdateEstadoSolicitudAsync(id, request.Estado);
            if (solicitud == null)
                return NotFound(new { message = "Solicitud no encontrada" });
            
            return Ok(new { message = "Estado de solicitud actualizado" });
        }

        [HttpPut("solicitud/{id}/completar")]
        public async Task<IActionResult> CompletarSolicitud(int id, [FromBody] CompletarSolicitudRequest request)
        {
            var solicitud = await _serviciosData.CompletarSolicitudAsync(id, request.NotasAdmin);
            if (solicitud == null)
                return NotFound(new { message = "Solicitud no encontrada" });
            
            return Ok(new { message = "Solicitud completada" });
        }

        // ==========================================
        // 5. CARGOS SERVICIO - 🟩 AMBAS (2 APIs)
        // ==========================================

        [HttpGet("cargos/servicios/usuario/{usuarioId}")]
        public async Task<IActionResult> GetCargosServiciosByUsuario(int usuarioId)
        {
            var cargos = await _serviciosData.GetCargosServiciosByUsuarioAsync(usuarioId);
            return Ok(cargos);
        }

        [HttpGet("cargos/servicios/solicitud/{solicitudId}")]
        public async Task<IActionResult> GetCargosServiciosBySolicitud(int solicitudId)
        {
            var cargos = await _serviciosData.GetCargosServiciosBySolicitudAsync(solicitudId);
            return Ok(cargos);
        }

        // ==========================================
        // 6. CARGOS MANTENIMIENTO - 🟩 AMBAS (4 APIs)
        // ==========================================

        [HttpGet("cargos/mantenimiento")]
        public async Task<IActionResult> GetCargosMantenimiento()
        {
            var cargos = await _serviciosData.GetCargosMantenimientoAsync();
            return Ok(cargos);
        }

        [HttpGet("cargos/mantenimiento/usuario/{usuarioId}")]
        public async Task<IActionResult> GetCargosMantenimientoByUsuario(int usuarioId)
        {
            var cargos = await _serviciosData.GetCargosMantenimientoByUsuarioAsync(usuarioId);
            return Ok(cargos);
        }

        [HttpPost("cargos/mantenimiento")]
        public async Task<IActionResult> CreateCargoMantenimiento([FromBody] CargoMantenimientoRequest request)
        {
            var cargo = await _serviciosData.CreateCargoMantenimientoAsync(request);
            return Ok(new { message = "Cargo creado exitosamente", id = cargo.CargoMantenimientoID });
        }

        [HttpPut("cargos/mantenimiento/{id}")]
        public async Task<IActionResult> UpdateCargoMantenimiento(int id, [FromBody] CargoMantenimientoRequest request)
        {
            var cargo = await _serviciosData.UpdateCargoMantenimientoAsync(id, request);
            if (cargo == null)
                return NotFound(new { message = "Cargo no encontrado" });
            
            return Ok(new { message = "Cargo actualizado exitosamente" });
        }
    }

    // ==========================================
    // REQUEST MODELS
    // ==========================================

    public class ServicioCatalogoRequest
    {
        public int TipoServicioID { get; set; }
        public string NombreEncargado { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NotasInternas { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class UpdateDisponibilidadRequest
    {
        public bool Disponible { get; set; }
    }

    public class PersonalMantenimientoRequest
    {
        public int PersonaID { get; set; }
        public string Puesto { get; set; } = string.Empty;
        public DateOnly FechaContratacion { get; set; }
        public decimal Sueldo { get; set; }
        public string? TipoContrato { get; set; }
        public string? Turno { get; set; }
        public string? DiasLaborales { get; set; }
        public string? Notas { get; set; }
    }

    public class SolicitudServicioRequest
    {
        public int UsuarioID { get; set; }
        public int TipoServicioID { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Urgencia { get; set; } = "Media";
        public DateOnly? FechaPreferida { get; set; }
        public TimeSpan? HoraPreferida { get; set; }
    }

    public class AsignarSolicitudRequest
    {
        public int PersonaAsignado { get; set; }
    }

    public class UpdateEstadoSolicitudRequest
    {
        public string Estado { get; set; } = string.Empty;
    }

    public class CompletarSolicitudRequest
    {
        public string? NotasAdmin { get; set; }
    }

    public class CargoMantenimientoRequest
    {
        public int? UsuarioID { get; set; }
        public int? PersonalMantenimientoID { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateOnly FechaVencimiento { get; set; }
    }
}