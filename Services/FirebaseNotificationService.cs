using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_RSV.Services
{
    public class FirebaseNotificationService
    {
        private readonly AppDbContext _context;

        public FirebaseNotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendPanicAlertNotification(AlertaPanico alerta)
        {
            try
            {
                // Obtener info del usuario
                var usuario = await _context.Usuarios
                    .Include(u => u.Persona)
                    .FirstOrDefaultAsync(u => u.UsuarioID == alerta.UsuarioID);

                if (usuario?.Persona == null)
                {
                    Console.WriteLine("Usuario no encontrado para notificación");
                    return;
                }

                string nombreUsuario = usuario.Persona.Nombre + " " + usuario.Persona.ApellidoPaterno;

                // Configurar mensaje de notificación
                var message = new Message()
                {
                    Topic = "panic_alerts", // Topic para alertas de panico
                    Notification = new Notification()
                    {
                        Title = "ALERTA DE PANICO ACTIVADA",
                        Body = $"{nombreUsuario} activo una alerta de emergencia"
                    },
                    Data = new Dictionary<string, string>()
                    {
                        { "action", "panic_alert" },
                        { "alertaId", alerta.AlertaID.ToString() },
                        { "usuarioId", alerta.UsuarioID.ToString() },
                        { "latitud", alerta.Latitud.ToString() },
                        { "longitud", alerta.Longitud.ToString() },
                        { "timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
                    },
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            ChannelId = "panic_alerts_channel",
                            Sound = "emergency_sound",
                            Priority = NotificationPriority.MAX
                        }
                    },
                    Apns = new ApnsConfig()
                    {
                        Headers = new Dictionary<string, string>()
                        {
                            { "apns-priority", "10" } // Maxima prioridad iOS
                        },
                        Aps = new Aps()
                        {
                            Sound = "emergency.caf",
                            ContentAvailable = true
                        }
                    }
                };

                // Enviar notificacion push
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Notificacion FCM enviada exitosamente: {response}");
                Console.WriteLine($"Topic: panic_alerts");
                Console.WriteLine($"Usuario: {nombreUsuario}");
                Console.WriteLine($"Ubicacion: {alerta.Latitud}, {alerta.Longitud}");

            }
            catch (FirebaseMessagingException fcmEx)
            {
                Console.WriteLine($"Error FCM: {fcmEx.Message}");
                Console.WriteLine($"Error Code: {fcmEx.ErrorCode}");
                Console.WriteLine($"Modo simulacion: Alerta {alerta.AlertaID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                Console.WriteLine($"Modo simulacion: Alerta {alerta.AlertaID}");
            }
        }
    }
}