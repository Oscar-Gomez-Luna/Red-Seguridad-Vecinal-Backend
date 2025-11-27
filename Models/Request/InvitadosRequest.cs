using System;

namespace Backend_RSV.Models.Request
{
    public class ValidacionQRResult
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool EsEntrada { get; set; }
    }

    public class CrearInvitadoRequest
    {
        public int UsuarioID { get; set; }
        public string NombreInvitado { get; set; } = string.Empty;
        public string ApellidoPaternoInvitado { get; set; } = string.Empty;
        public string ApellidoMaternoInvitado { get; set; } = string.Empty;
        public DateTime FechaVisita { get; set; }
    }
}