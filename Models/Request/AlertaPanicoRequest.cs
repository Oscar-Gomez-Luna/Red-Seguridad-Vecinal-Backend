using System;

namespace Backend_RSV.Models.Request
{
    public class AlertaPanicoRequest
    {
        public int UsuarioID { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }

    public class UpdateEstadoRequest
    {
        public string Estado { get; set; } = string.Empty;
    }
}