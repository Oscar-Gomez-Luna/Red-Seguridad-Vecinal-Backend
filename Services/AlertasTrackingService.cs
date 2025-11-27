using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend_RSV.Data.Alertas;

namespace Backend_RSV.Services
{
    // Objeto auxiliar para seguimiento de cada alerta
    public class PendingAlertTracker
    {
        public DateTime StartTime { get; set; }
        public DateTime LastCheckTime { get; set; }
        public string FirebaseId { get; set; } = string.Empty;
        public decimal LastLat { get; set; }
        public decimal LastLng { get; set; }
    }

    public interface IAlertasTrackingService
    {
        void AgregarAlerta(int alertaId, string firebaseId, decimal lat, decimal lng);
    }

    public class AlertasTrackingService : BackgroundService, IAlertasTrackingService
    {
        private readonly ConcurrentDictionary<int, PendingAlertTracker> _alertasPendientes;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly TimeSpan _loopInterval = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _maxDuration = TimeSpan.FromMinutes(30);

        public AlertasTrackingService(IServiceScopeFactory scopeFactory)
        {
            _alertasPendientes = new ConcurrentDictionary<int, PendingAlertTracker>();
            _scopeFactory = scopeFactory;
        }

        public void AgregarAlerta(int alertaId, string firebaseId, decimal lat, decimal lng)
        {
            var tracker = new PendingAlertTracker
            {
                StartTime = DateTime.Now,
                LastCheckTime = DateTime.Now,
                FirebaseId = firebaseId,
                LastLat = lat,
                LastLng = lng
            };
            _alertasPendientes.TryAdd(alertaId, tracker);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var kvp in _alertasPendientes.ToList())
                    {
                        int alertaId = kvp.Key;
                        PendingAlertTracker tracker = kvp.Value;

                        if (DateTime.Now - tracker.LastCheckTime >= _checkInterval)
                        {
                            await RevisarAlerta(alertaId, tracker);
                            tracker.LastCheckTime = DateTime.Now;
                        }

                        if (DateTime.Now - tracker.StartTime >= _maxDuration)
                        {
                            await CerrarAlerta(alertaId, tracker);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en AlertasTrackingService: {ex.Message}");
                }

                await Task.Delay(_loopInterval, stoppingToken);
            }
        }

        private async Task RevisarAlerta(int alertaId, PendingAlertTracker tracker)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var alertaData = scope.ServiceProvider.GetRequiredService<AlertaPanicoData>();
                var firebaseService = scope.ServiceProvider.GetRequiredService<IFirebaseDataService>();

                var alerta = await alertaData.GetAlertaByIdAsync(alertaId);
                if (alerta == null)
                {
                    _alertasPendientes.TryRemove(alertaId, out _);
                    return;
                }

                var firebaseAlerta = await firebaseService.ObtenerAlertaFirebase(tracker.FirebaseId);

                if (firebaseAlerta.estatus != "activa")
                {
                    _alertasPendientes.TryRemove(alertaId, out _);
                    return;
                }

                decimal nuevaLat = (decimal)firebaseAlerta.ubicacion.lat;
                decimal nuevaLng = (decimal)firebaseAlerta.ubicacion.lng;

                if (nuevaLat != tracker.LastLat || nuevaLng != tracker.LastLng)
                {
                    await alertaData.UpdateAlertaCoordenadasAsync(alertaId, nuevaLat, nuevaLng);
                    tracker.LastLat = nuevaLat;
                    tracker.LastLng = nuevaLng;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al revisar alerta {alertaId}: {ex.Message}");
            }
        }

        private async Task CerrarAlerta(int alertaId, PendingAlertTracker tracker)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var alertaData = scope.ServiceProvider.GetRequiredService<AlertaPanicoData>();
                var firebaseService = scope.ServiceProvider.GetRequiredService<IFirebaseDataService>();

                await firebaseService.ActualizarEstatusAlerta(tracker.FirebaseId, "cancelada");
                await alertaData.UpdateAlertaEstatusAsync(alertaId, "cancelada");
                _alertasPendientes.TryRemove(alertaId, out _);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar alerta {alertaId}: {ex.Message}");
            }
        }
    }

}
