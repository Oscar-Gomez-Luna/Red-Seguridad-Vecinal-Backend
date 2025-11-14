// Services/FirebaseDataService.cs
using Firebase.Database;
using Firebase.Database.Query;
using Backend_RSV.Models;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Backend_RSV.Services
{
    public interface IFirebaseDataService
    {
        Task<string> GuardarAlertaEnFirebase(AlertaPanico alerta);
        Task<bool> ActualizarEstatusAlerta(string firebaseId, string nuevoEstatus);
        Task<bool> EliminarAlertaFirebase(string firebaseId);
    }

    public class FirebaseDataService : IFirebaseDataService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public FirebaseDataService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            // Inicializar Firebase Admin SDK si no está inicializado
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
                });
            }

            // Obtener el token de acceso para Firebase Database
            var accessToken = GetAccessTokenAsync().Result;
            
            // Configuración del cliente Firebase con autenticación
            _firebaseClient = new FirebaseClient(
                "https://red-seguridad-vecinal-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(accessToken)
                });
        }

        private async Task<string> GetAccessTokenAsync()
        {
            try
            {
                // Obtener token de acceso usando las credenciales del archivo JSON
                var credential = GoogleCredential.FromFile("firebase-adminsdk.json")
                    .CreateScoped("https://www.googleapis.com/auth/firebase.database");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                return accessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo token de acceso: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GuardarAlertaEnFirebase(AlertaPanico alerta)
        {
            try
            {
                // Obtener información del usuario
                var usuario = await _context.Usuarios
                    .Include(u => u.Persona)
                    .FirstOrDefaultAsync(u => u.UsuarioID == alerta.UsuarioID);

                if (usuario?.Persona == null)
                    throw new Exception("Usuario no encontrado");

                string nombreCompleto = $"{usuario.Persona.Nombre} {usuario.Persona.ApellidoPaterno}";

                // Crear objeto para Firebase
                var firebaseAlerta = new FirebaseAlertaPanico
                {
                    id_alerta_ejemplo = $"alerta_{alerta.AlertaID}_{DateTime.Now:yyyyMMddHHmmss}",
                    estatus = "activa",
                    nombre_usuario = nombreCompleto,
                    timestamp = ((DateTimeOffset)alerta.FechaHora).ToUnixTimeSeconds(),
                    ubicacion = new Ubicacion
                    {
                        lat = (double)alerta.Latitud,
                        lng = (double)alerta.Longitud
                    },
                    uid = alerta.UsuarioID.ToString()
                };

                // Guardar en Firebase
                var result = await _firebaseClient
                    .Child("alertas_panico")
                    .PostAsync(firebaseAlerta);

                Console.WriteLine($"Alerta guardada en Firebase con ID: {result.Key}");
                return result.Key; // Retorna el ID generado por Firebase
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar en Firebase: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ActualizarEstatusAlerta(string firebaseId, string nuevoEstatus)
        {
            try
            {
                await _firebaseClient
                    .Child("alertas_panico")
                    .Child(firebaseId)
                    .Child("estatus")
                    .PutAsync(nuevoEstatus);

                Console.WriteLine($"Estatus actualizado en Firebase para ID: {firebaseId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estatus en Firebase: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarAlertaFirebase(string firebaseId)
        {
            try
            {
                await _firebaseClient
                    .Child("alertas_panico")
                    .Child(firebaseId)
                    .DeleteAsync();

                Console.WriteLine($"Alerta eliminada de Firebase con ID: {firebaseId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar alerta en Firebase: {ex.Message}");
                return false;
            }
        }
    }
}