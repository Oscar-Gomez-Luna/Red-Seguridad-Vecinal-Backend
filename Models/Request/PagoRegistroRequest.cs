using System.ComponentModel.DataAnnotations;

namespace Backend_RSV.Models.Request
{
    public class PagoRegistroRequest
    {
        [Required]
        public int UsuarioID { get; set; }

        public int? CargoMantenimientoID { get; set; }
        public int? CargoServicioID { get; set; }

        [Required]
        public string FolioUnico { get; set; } = string.Empty;

        [Required]
        public decimal MontoTotal { get; set; }

        [Required]
        public string TipoPago { get; set; } = string.Empty;

        public string MetodoPago { get; set; } = "Debito";

        public string? UltimosDigitosTarjeta { get; set; }

        // Lista de detalles de pago enviada desde el front
        public string? DetallesPagoJson { get; set; }
    }

    public class DetallePagoRequest
    {
        public string TipoCargo { get; set; } = string.Empty;
        public int CargoID { get; set; }
        public decimal MontoAplicado { get; set; }
    }

}