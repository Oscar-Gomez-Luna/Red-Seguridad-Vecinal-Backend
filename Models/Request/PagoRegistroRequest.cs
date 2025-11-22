using System.ComponentModel.DataAnnotations;

namespace Backend_RSV.Models.Request
{
    public class PagoRegistroRequest
    {
        [Required]
        public int UsuarioID { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }

        [Required]
        public string TipoPago { get; set; } = string.Empty;

        public string MetodoPago { get; set; } = "Debito";

        public string? DetallesPagoJson { get; set; }
    }


    public class DetallePagoRequest
    {
        public int? CargoMantenimientoID { get; set; }
        public int? CargoServicioID { get; set; }
        public decimal MontoAplicado { get; set; }
    }

}