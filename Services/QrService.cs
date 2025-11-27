using System;

namespace Backend_RSV.Services
{
    public class QrService
    {
        public string GenerarQRPersonal(int usuarioId)
        {
            return $"RSV_P_{usuarioId}_{Guid.NewGuid():N}_{DateTime.Now:yyyyMMdd}";
        }

        public string GenerarQRInvitado(int usuarioId, string nombreInvitado)
        {
            return $"RSV_I_{usuarioId}_{Guid.NewGuid():N}_{DateTime.Now:yyyyMMddHHmm}";
        }

        public (string tipo, int usuarioId) DecodificarQR(string codigoQR)
        {
            var partes = codigoQR.Split('_');
            if (partes.Length >= 3 && partes[0] == "RSV")
            {
                return (partes[1], int.Parse(partes[2]));
            }
            throw new ArgumentException("QR inválido");
        }
    }
}