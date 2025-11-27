using System;

namespace Backend_RSV.Models.Request
{
    public class ReporteRequest
    {
        public int? UsuarioID { get; set; }
        public int TipoReporteID { get; set; }
        public string? Titulo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? DireccionTexto { get; set; }
        public bool EsAnonimo { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    public class CambiarAnonimatoRequest
    {
        public bool EsAnonimo { get; set; }
    }

    public class UpdateEstadoReporteRequest
    {
        public bool Visto { get; set; }
    }
}