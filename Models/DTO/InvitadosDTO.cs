using System;

namespace Backend_RSV.Models.DTO
{
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
}