using System;

namespace Backend_RSV.Models.DTO
{
    public class ReporteDTO
    {
        public int ReporteID { get; set; }
        public int? UsuarioID { get; set; }
        public int TipoReporteID { get; set; }
        public string? Titulo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? DireccionTexto { get; set; }
        public bool EsAnonimo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Visto { get; set; }
        public string? Imagen { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string TipoReporte { get; set; } = string.Empty;
    }

    public class ReporteDetailDTO
    {
        public int ReporteID { get; set; }
        public int? UsuarioID { get; set; }
        public int TipoReporteID { get; set; }
        public string? Titulo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? DireccionTexto { get; set; }
        public bool EsAnonimo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Visto { get; set; }
        public string? Imagen { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string TipoReporte { get; set; } = string.Empty;
        public string NumeroCasa { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
    }
}