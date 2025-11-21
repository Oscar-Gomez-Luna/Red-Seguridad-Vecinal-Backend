using Backend_RSV.Models.Request;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_RSV.Data.Estadisticas
{
    public class EstadisticasData
    {
        private readonly AppDbContext _context;

        public EstadisticasData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> ObtenerEstadisticasIncidentesAsync()
        {
            var totalReportes = await _context.Reportes.CountAsync();

            var porTipo = await _context.Reportes
                .GroupBy(r => r.TipoReporte.Nombre)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            var porMes = _context.Reportes
                .AsEnumerable()
                .GroupBy(r => new { r.FechaCreacion.Year, r.FechaCreacion.Month })
                .Select(g => new
                {
                    Mes = $"{g.Key.Month:D2}/{g.Key.Year}",
                    Total = g.Count()
                })
                .OrderBy(g => g.Mes)
                .ToList();

            var anonimos = await _context.Reportes.CountAsync(r => r.EsAnonimo);
            var vistos = await _context.Reportes.CountAsync(r => r.Visto);

            return new
            {
                TotalReportes = totalReportes,
                PorTipo = porTipo,
                PorMes = porMes,
                Anonimos = anonimos,
                Identificados = totalReportes - anonimos,
                Vistos = vistos,
                NoVistos = totalReportes - vistos
            };
        }

        public async Task<object> ObtenerEstadisticasPagosAsync()
        {
            var totalPagos = await _context.Pagos.CountAsync();
            var totalRecaudado = await _context.Pagos.SumAsync(p => p.MontoTotal);

            var porTipoPago = await _context.Pagos
                .GroupBy(p => p.TipoPago)
                .Select(g => new
                {
                    TipoPago = g.Key,
                    Total = g.Count(),
                    MontoTotal = g.Sum(p => p.MontoTotal)
                })
                .ToListAsync();

            var porMetodoPago = await _context.Pagos
                .GroupBy(p => p.MetodoPago)
                .Select(g => new
                {
                    Metodo = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            var porMes = _context.Pagos
                .AsEnumerable()
                .GroupBy(p => new { p.FechaPago.Year, p.FechaPago.Month })
                .Select(g => new
                {
                    Mes = $"{g.Key.Month:D2}/{g.Key.Year}",
                    TotalPagos = g.Count(),
                    MontoTotal = g.Sum(p => p.MontoTotal)
                })
                .OrderBy(g => g.Mes)
                .ToList();

            var adeudosServicios = await _context.CargosServicios
                .Where(c => c.Estado == "Pendiente")
                .SumAsync(c => c.SaldoPendiente);

            var adeudosMantenimiento = await _context.CargosMantenimiento
                .Where(c => c.Estado == "Pendiente")
                .SumAsync(c => c.SaldoPendiente);

            var adeudoTotal = adeudosServicios + adeudosMantenimiento;

            return new
            {
                TotalPagos = totalPagos,
                TotalRecaudado = totalRecaudado,
                PorTipoPago = porTipoPago,
                PorMetodoPago = porMetodoPago,
                PorMes = porMes,
                AdeudoTotal = adeudoTotal
            };
        }
        public async Task<object> ObtenerEstadisticasServiciosAsync()
        {
            var totalServicios = await _context.ServiciosCatalogo.CountAsync(s => s.Activo);
            var disponibles = await _context.ServiciosCatalogo.CountAsync(s => s.Disponible);
            var noDisponibles = totalServicios - disponibles;

            var porTipo = await _context.ServiciosCatalogo
                .GroupBy(s => s.TipoServicio.Nombre)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Total = g.Count(),
                    Completados = g.Sum(x => x.NumeroServiciosCompletados)
                })
                .ToListAsync();

            var totalesCargos = await _context.CargosServicios
                .GroupBy(c => c.Estado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Total = g.Count(),
                    MontoTotal = g.Sum(c => c.Monto)
                })
                .ToListAsync();

            var porMes = _context.CargosServicios
               .AsEnumerable()
               .GroupBy(c => new { c.FechaCreacion.Year, c.FechaCreacion.Month })
               .Select(g => new
               {
                   Mes = $"{g.Key.Month:D2}/{g.Key.Year}",
                   TotalSolicitudes = g.Count()
               })
               .OrderBy(g => g.Mes)
               .ToList();

            var totalCompletados = await _context.ServiciosCatalogo.SumAsync(s => s.NumeroServiciosCompletados);

            return new
            {
                TotalServicios = totalServicios,
                Disponibles = disponibles,
                NoDisponibles = noDisponibles,
                TotalCompletados = totalCompletados,
                PorTipo = porTipo,
                TotalesCargos = totalesCargos,
                PorMes = porMes
            };
        }
        public async Task<object> ObtenerEstadisticasPorTipoAsync(string tipo = "todo", DateTime? desde = null, DateTime? hasta = null)
        {
            tipo = tipo?.ToLower() ?? "todo";
            var resultado = new Dictionary<string, object>();

            if (tipo == "incidentes" || tipo == "todo")
                resultado["incidentes"] = await ObtenerEstadisticasIncidentesAsync();

            if (tipo == "pagos" || tipo == "todo")
                resultado["pagos"] = await ObtenerEstadisticasPagosAsync();

            if (tipo == "servicios" || tipo == "todo")
                resultado["servicios"] = await ObtenerEstadisticasServiciosAsync();

            return resultado;
        }
        public async Task<EstadisticasGenerales> ObtenerEstadisticasGeneralesAsync()
        {
            var incidentes = await ObtenerEstadisticasIncidentesAsync();
            var pagos = await ObtenerEstadisticasPagosAsync();
            var servicios = await ObtenerEstadisticasServiciosAsync();

            return new EstadisticasGenerales
            {
                Incidentes = incidentes,
                Pagos = pagos,
                Servicios = servicios
            };
        }
    }
}