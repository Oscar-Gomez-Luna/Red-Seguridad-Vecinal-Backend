using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using Backend_RSV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_RSV.Data.Invitados
{
    public class InvitadosData
    {
        private readonly AppDbContext _context;
        private readonly QrService _qrService;

        public InvitadosData(AppDbContext context, QrService qrService)
        {
            _context = context;
            _qrService = qrService;
        }

        // ==========================================
        // INVITADOS - 🟨 ANDROID
        // ==========================================

        public async Task<Invitado> CrearInvitadoAsync(CrearInvitadoRequest request)
        {
            // SOLO CADENA DE TEXTO - NO BASE64
            var qrTexto = _qrService.GenerarQRInvitado(request.UsuarioID, request.NombreInvitado);

            var invitado = new Invitado
            {
                UsuarioID = request.UsuarioID,
                NombreInvitado = request.NombreInvitado,
                ApellidoPaternoInvitado = request.ApellidoPaternoInvitado,
                ApellidoMaternoInvitado = request.ApellidoMaternoInvitado,
                CodigoQR = qrTexto, // ← SOLO TEXTO
                FechaGeneracion = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddHours(24)
            };

            _context.Invitados.Add(invitado);
            await _context.SaveChangesAsync();
            return invitado;
        }

        public async Task<List<InvitadoDTO>> GetInvitadosByUsuarioAsync(int usuarioId)
        {
            return await _context.Invitados
                .Where(i => i.UsuarioID == usuarioId)
                .OrderByDescending(i => i.FechaGeneracion)
                .Select(i => new InvitadoDTO
                {
                    InvitadoID = i.InvitadoID,
                    NombreInvitado = i.NombreInvitado,
                    ApellidoPaternoInvitado = i.ApellidoPaternoInvitado,
                    ApellidoMaternoInvitado = i.ApellidoMaternoInvitado,
                    FechaGeneracion = i.FechaGeneracion,
                    FechaVencimiento = i.FechaVencimiento,
                    FechaEntrada = i.FechaEntrada,
                    FechaSalida = i.FechaSalida,
                    Estado = GetEstadoInvitado(i), // ← MÉTODO ESTÁTICO
                    CodigoQR = i.CodigoQR
                })
                .ToListAsync();
        }

        public async Task<bool> CancelarInvitacionAsync(int invitadoId)
        {
            var invitado = await _context.Invitados
                .FirstOrDefaultAsync(i => i.InvitadoID == invitadoId);

            if (invitado == null) return false;

            if (invitado.FechaEntrada == null && invitado.FechaSalida == null)
            {
                _context.Invitados.Remove(invitado);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<InvitadoDTO>> GetAllInvitadosAsync()
        {
            return await _context.Invitados
                .Include(i => i.Usuario)
                    .ThenInclude(u => u.Persona)
                .OrderByDescending(i => i.FechaGeneracion)
                .Select(i => new InvitadoDTO
                {
                    InvitadoID = i.InvitadoID,
                    NombreInvitado = i.NombreInvitado,
                    ApellidoPaternoInvitado = i.ApellidoPaternoInvitado,
                    ApellidoMaternoInvitado = i.ApellidoMaternoInvitado,
                    FechaGeneracion = i.FechaGeneracion,
                    FechaVencimiento = i.FechaVencimiento,
                    FechaEntrada = i.FechaEntrada,
                    FechaSalida = i.FechaSalida,
                    Estado = GetEstadoInvitado(i), // ← MÉTODO ESTÁTICO
                    CodigoQR = i.CodigoQR,
                    NombreResidente = i.Usuario.Persona.Nombre + " " + i.Usuario.Persona.ApellidoPaterno,
                    NumeroCasa = i.Usuario.NumeroCasa
                })
                .ToListAsync();
        }

        // ==========================================
        // ACCESOS - 🟦 PWA
        // ==========================================

        public async Task<ValidacionQRResult> ValidarQRAsync(string codigoQR)
        {
            try
            {
                var (tipo, usuarioId) = _qrService.DecodificarQR(codigoQR);

                if (tipo == "I") // QR de Invitado
                {
                    return await ValidarQRInvitadoAsync(codigoQR);
                }
                else if (tipo == "P") // QR Personal
                {
                    return await ValidarQRPersonalAsync(codigoQR, usuarioId);
                }

                return new ValidacionQRResult { Exitoso = false, Mensaje = "Tipo de QR no válido" };
            }
            catch (Exception)
            {
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR inválido o corrupto" };
            }
        }

        private async Task<ValidacionQRResult> ValidarQRInvitadoAsync(string codigoQR)
        {
            var invitado = await _context.Invitados
                .Include(i => i.Usuario)
                    .ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(i => i.CodigoQR == codigoQR);

            if (invitado == null)
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR no encontrado" };

            if (DateTime.Now > invitado.FechaVencimiento)
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR expirado" };

            // Lógica de entrada/salida
            if (invitado.FechaEntrada == null)
            {
                // PRIMER USO - ENTRADA
                invitado.FechaEntrada = DateTime.Now;
                await _context.SaveChangesAsync();

                return new ValidacionQRResult 
                { 
                    Exitoso = true, 
                    Mensaje = "Entrada registrada exitosamente",
                    Tipo = "Invitado",
                    Nombre = $"{invitado.NombreInvitado} {invitado.ApellidoPaternoInvitado}",
                    EsEntrada = true
                };
            }
            else if (invitado.FechaSalida == null)
            {
                // SEGUNDO USO - SALIDA
                invitado.FechaSalida = DateTime.Now;
                await _context.SaveChangesAsync();

                return new ValidacionQRResult 
                { 
                    Exitoso = true, 
                    Mensaje = "Salida registrada exitosamente",
                    Tipo = "Invitado", 
                    Nombre = $"{invitado.NombreInvitado} {invitado.ApellidoPaternoInvitado}",
                    EsEntrada = false
                };
            }
            else
            {
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR ya utilizado completamente" };
            }
        }

        private async Task<ValidacionQRResult> ValidarQRPersonalAsync(string codigoQR, int usuarioId)
        {
            var qrPersonal = await _context.QRPersonales
                .Include(q => q.Usuario)
                    .ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(q => q.CodigoQR == codigoQR && q.UsuarioID == usuarioId);

            if (qrPersonal == null)
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR personal no encontrado" };

            if (!qrPersonal.Activo)
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR personal desactivado" };

            if (DateTime.Now > qrPersonal.FechaVencimiento)
                return new ValidacionQRResult { Exitoso = false, Mensaje = "QR personal expirado" };

            return new ValidacionQRResult 
            { 
                Exitoso = true, 
                Mensaje = "Acceso permitido",
                Tipo = "Personal",
                Nombre = $"{qrPersonal.Usuario.Persona.Nombre} {qrPersonal.Usuario.Persona.ApellidoPaterno}",
                EsEntrada = true
            };
        }

        public async Task<List<AccesoHistorialDTO>> GetHistorialAccesosAsync()
        {
            var invitadosConAcceso = await _context.Invitados
                .Where(i => i.FechaEntrada != null)
                .Include(i => i.Usuario)
                    .ThenInclude(u => u.Persona)
                .Select(i => new AccesoHistorialDTO
                {
                    ID = i.InvitadoID,
                    Tipo = "Invitado",
                    Nombre = $"{i.NombreInvitado} {i.ApellidoPaternoInvitado}",
                    Residente = $"{i.Usuario.Persona.Nombre} {i.Usuario.Persona.ApellidoPaterno}",
                    FechaAcceso = i.FechaEntrada.Value,
                    TipoAcceso = "Entrada",
                    NumeroCasa = i.Usuario.NumeroCasa
                })
                .ToListAsync();

            var invitadosConSalida = await _context.Invitados
                .Where(i => i.FechaSalida != null)
                .Include(i => i.Usuario)
                    .ThenInclude(u => u.Persona)
                .Select(i => new AccesoHistorialDTO
                {
                    ID = i.InvitadoID,
                    Tipo = "Invitado",
                    Nombre = $"{i.NombreInvitado} {i.ApellidoPaternoInvitado}",
                    Residente = $"{i.Usuario.Persona.Nombre} {i.Usuario.Persona.ApellidoPaterno}",
                    FechaAcceso = i.FechaSalida.Value,
                    TipoAcceso = "Salida",
                    NumeroCasa = i.Usuario.NumeroCasa
                })
                .ToListAsync();

            return invitadosConAcceso
                .Concat(invitadosConSalida)
                .OrderByDescending(a => a.FechaAcceso)
                .ToList();
        }

        public async Task<List<AccesoHistorialDTO>> GetAccesosByUsuarioAsync(int usuarioId)
        {
            return await _context.Invitados
                .Where(i => i.UsuarioID == usuarioId && i.FechaEntrada != null)
                .Select(i => new AccesoHistorialDTO
                {
                    ID = i.InvitadoID,
                    Tipo = "Invitado",
                    Nombre = $"{i.NombreInvitado} {i.ApellidoPaternoInvitado}",
                    Residente = $"{i.Usuario.Persona.Nombre} {i.Usuario.Persona.ApellidoPaterno}",
                    FechaAcceso = i.FechaEntrada.Value,
                    TipoAcceso = "Entrada",
                    NumeroCasa = i.Usuario.NumeroCasa
                })
                .OrderByDescending(a => a.FechaAcceso)
                .ToListAsync();
        }

        // ==========================================
        // QR PERSONAL - 🟨 ANDROID & 🟦 PWA
        // ==========================================

        public async Task<QRPersonal> GenerarQRPersonalAsync(int usuarioId)
        {
            // Desactivar QR anteriores del usuario
            var qrsAnteriores = await _context.QRPersonales
                .Where(q => q.UsuarioID == usuarioId && q.Activo)
                .ToListAsync();

            foreach (var qr in qrsAnteriores)
            {
                qr.Activo = false;
            }

            // SOLO CADENA DE TEXTO - NO BASE64
            var qrTexto = _qrService.GenerarQRPersonal(usuarioId);

            var qrPersonal = new QRPersonal
            {
                UsuarioID = usuarioId,
                CodigoQR = qrTexto, // ← SOLO TEXTO
                FechaGeneracion = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddYears(1),
                Activo = true
            };

            _context.QRPersonales.Add(qrPersonal);
            await _context.SaveChangesAsync();
            return qrPersonal;
        }

        public async Task<QRPersonalDTO?> GetQRPersonalByUsuarioAsync(int usuarioId)
        {
            return await _context.QRPersonales
                .Where(q => q.UsuarioID == usuarioId && q.Activo)
                .Select(q => new QRPersonalDTO
                {
                    QRID = q.QRID,
                    UsuarioID = q.UsuarioID,
                    CodigoQR = q.CodigoQR,
                    FechaGeneracion = q.FechaGeneracion,
                    FechaVencimiento = q.FechaVencimiento,
                    Activo = q.Activo
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateEstadoQRAsync(int qrId, bool activo)
        {
            var qrPersonal = await _context.QRPersonales
                .FirstOrDefaultAsync(q => q.QRID == qrId);

            if (qrPersonal == null) return false;

            qrPersonal.Activo = activo;
            await _context.SaveChangesAsync();

            return true;
        }

        // ==========================================
        // MÉTODOS AUXILIARES - CORREGIDOS
        // ==========================================

        // MÉTODO CORREGIDO - AHORA ES ESTÁTICO
        private static string GetEstadoInvitado(Invitado invitado)
        {
            if (invitado.FechaSalida != null) return "Completado";
            if (invitado.FechaEntrada != null) return "Dentro";
            if (DateTime.Now > invitado.FechaVencimiento) return "Expirado";
            return "Pendiente";
        }
    }

    // ==========================================
    // DTOs
    // ==========================================

    public class InvitadoDTO
    {
        public int InvitadoID { get; set; }
        public string NombreInvitado { get; set; } = string.Empty;
        public string ApellidoPaternoInvitado { get; set; } = string.Empty;
        public string ApellidoMaternoInvitado { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string CodigoQR { get; set; } = string.Empty;
        public string? NombreResidente { get; set; }
        public string? NumeroCasa { get; set; }
    }

    public class QRPersonalDTO
    {
        public int QRID { get; set; }
        public int UsuarioID { get; set; }
        public string CodigoQR { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Activo { get; set; }
    }

    public class AccesoHistorialDTO
    {
        public int ID { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Residente { get; set; } = string.Empty;
        public DateTime FechaAcceso { get; set; }
        public string TipoAcceso { get; set; } = string.Empty;
        public string? NumeroCasa { get; set; }
    }

    public class ValidacionQRResult
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool EsEntrada { get; set; }
    }

    // Request Models
    public class CrearInvitadoRequest
    {
        public int UsuarioID { get; set; }
        public string NombreInvitado { get; set; } = string.Empty;
        public string ApellidoPaternoInvitado { get; set; } = string.Empty;
        public string ApellidoMaternoInvitado { get; set; } = string.Empty;
        public DateTime FechaVisita { get; set; }
    }
}