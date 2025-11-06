using System;

namespace Backend_RSV.Data.Alertas
{
    public class AlertaPanicoDTO
    {
        public int AlertaID { get; set; }
        public int UsuarioID { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaHora { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
    }

    public class AlertaPanicoDetailDTO
    {
        public int AlertaID { get; set; }
        public int UsuarioID { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaHora { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string NumeroCasa { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
    }
}