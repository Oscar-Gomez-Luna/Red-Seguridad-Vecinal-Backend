using System.Text;
using System.Text.Json;
using Backend_RSV.Data.Usuarios;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Backend_RSV.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UsuariosData _usuariosData;

        public UsuariosController(UsuariosData usuariosData, IConfiguration config)
        {
            _config = config;
            _usuariosData = usuariosData;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Email y contraseña son requeridos." });

            try
            {
                using var http = new HttpClient();
                var content = new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        email = request.Email,
                        password = request.Password,
                        returnSecureToken = true
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                var firebaseKey = _config["Firebase:ApiKey"];
                var response = await http.PostAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={firebaseKey}",
                    content
                );

                if (!response.IsSuccessStatusCode)
                    return Unauthorized(new { message = "Credenciales inválidas" });

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(json);
                var firebaseUID = data.GetProperty("localId").GetString();

                if (firebaseUID == null)
                    return NotFound(new { message = "Usuario no encontrado en la base de datos." });

                var usuario = await _usuariosData.LoginAsync(firebaseUID);

                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado en la base de datos." });

                return Ok(new
                {
                    message = "Inicio de sesión exitoso",
                    id = usuario.UsuarioID,
                    nombre = usuario.Persona.Nombre,
                    tipoUsuario = usuario.TipoUsuario.Nombre,
                    firebaseID = firebaseUID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            if (request == null)
                return BadRequest("Datos incompletos.");

            UserRecord? firebaseUser = null;

            try
            {
                firebaseUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
                {
                    Email = request.Email,
                    Password = request.Password,
                    DisplayName = $"{request.Nombre} {request.ApellidoPaterno}",
                    Disabled = false
                });

                var persona = new Persona
                {
                    Nombre = request.Nombre,
                    ApellidoPaterno = request.ApellidoPaterno,
                    ApellidoMaterno = request.ApellidoMaterno,
                    Telefono = request.Telefono,
                    Email = request.Email,
                    FechaNacimiento = request.FechaNacimiento
                };

                var usuario = new Usuario
                {
                    FirebaseUID = firebaseUser.Uid,
                    TipoUsuarioID = request.TipoUsuarioID,
                    NumeroCasa = request.NumeroCasa,
                    Calle = request.Calle,
                    Activo = true
                };

                var numeroTarjetaBytes = Encoding.UTF8.GetBytes(request.NumeroTarjeta);

                var cuenta = new CuentaUsuario
                {
                    NumeroTarjeta = numeroTarjetaBytes,
                    UltimosDigitos = request.UltimosDigitos,
                    FechaVencimiento = request.FechaVencimiento
                };

                var nuevoUsuario = await _usuariosData.RegistrarUsuarioAsync(persona, usuario, cuenta);

                return Ok(new
                {
                    message = "Usuario registrado correctamente",
                    id = nuevoUsuario.UsuarioID,
                    firebaseUID = firebaseUser.Uid,
                    email = firebaseUser.Email,
                    nombre = nuevoUsuario.Persona?.Nombre,
                    tipoUsuario = nuevoUsuario.TipoUsuario?.Nombre
                });
            }
            catch (Exception ex)
            {
                if (firebaseUser != null)
                {
                    try
                    {
                        await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUser.Uid);
                        Console.WriteLine($"Usuario Firebase {firebaseUser.Uid} eliminado por error SQL.");
                    }
                    catch (Exception deleteEx)
                    {
                        Console.WriteLine($"Error al revertir usuario Firebase: {deleteEx.Message}");
                    }
                }

                var inner = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = "Error al registrar usuario",
                    error = inner
                });
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUsuario([FromBody] UsuarioUpdateRequest request)
        {
            if (request == null || request.UsuarioID <= 0)
                return BadRequest(new { mensaje = "Datos de actualización o ID inválidos." });

            try
            {
                bool actualizado = await _usuariosData.UpdateUsuarioAsync(request);

                if (!actualizado)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new { mensaje = "Usuario actualizado correctamente." });
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { mensaje = "Error al actualizar en Firebase.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor.", detalle = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuariosData.GetAllUsuariosAsync();

            if (usuarios == null || !usuarios.Any())
                return NotFound(new { message = "No hay usuarios registrados." });

            return Ok(usuarios.Select(u => new
            {
                u.UsuarioID,
                u.Persona.Nombre,
                u.Persona.ApellidoPaterno,
                u.Persona.ApellidoMaterno,
                u.Persona.Email,
                tipoUsuario = u.TipoUsuario.Nombre,
                u.Activo
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuariosData.GetUsuarioByIdAsync(id);

            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado." });

            return Ok(new
            {
                usuario.UsuarioID,
                usuario.Persona.Nombre,
                usuario.Persona.ApellidoPaterno,
                usuario.Persona.ApellidoMaterno,
                usuario.Persona.Telefono,
                usuario.Persona.Email,
                usuario.Persona.FechaNacimiento,
                tipoUsuario = usuario.TipoUsuario.Nombre,
                usuario.NumeroCasa,
                usuario.Calle,
                usuario.Activo
            });
        }

        [HttpGet("tipos-usuario")]
        public async Task<IActionResult> GetTiposUsuario()
        {
            var tipos = await _usuariosData.GetTiposUsuarioAsync();

            if (tipos == null || !tipos.Any())
                return NotFound(new { message = "No hay tipos de usuario registrados." });

            return Ok(tipos.Select(t => new
            {
                t.TipoUsuarioID,
                t.Nombre,
                t.Descripcion
            }));
        }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class RegistroRequest
    {
        public int TipoUsuarioID { get; set; }
        public string? NumeroCasa { get; set; }
        public string? Calle { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NumeroTarjeta { get; set; } = string.Empty;
        public string UltimosDigitos { get; set; } = string.Empty;
        public DateOnly FechaVencimiento { get; set; }
    }
}