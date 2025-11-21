using System;

namespace Backend_RSV.Models.Request
{

    public class GenerarQRRequest
    {
        public int UsuarioID { get; set; }
    }

    public class UpdateEstadoQRRequest
    {
        public bool Activo { get; set; }
    }
}