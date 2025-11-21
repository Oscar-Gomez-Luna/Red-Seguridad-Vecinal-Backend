using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DetallePago
{
    [Key]
    public int DetalleID { get; set; }
    [Required]
    public int PagoID { get; set; }
    public int? CargoMantenimientoID { get; set; }
    public int? CargoServicioID { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal MontoAplicado { get; set; }
    public DateTime FechaAplicacion { get; set; } = DateTime.Now;
    public virtual Pago Pago { get; set; } = null!;
    public virtual CargoMantenimiento? CargoMantenimiento { get; set; }
    public virtual CargoServicio? CargoServicio { get; set; }
}