// Models/FirebaseAlertaPanico.cs
using System;

namespace Backend_RSV.Models
{
    public class FirebaseAlertaPanico
    {
        public string id_alerta_ejemplo { get; set; } = string.Empty;
        public string estatus { get; set; } = "activa";
        public string nombre_usuario { get; set; } = string.Empty;
        public long timestamp { get; set; }
        public Ubicacion ubicacion { get; set; } = new Ubicacion();
        public string uid { get; set; } = string.Empty;
    }

    public class Ubicacion
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}