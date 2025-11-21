using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CargoServicio
{
    [Key]
    public int CargoServicioID { get; set; }

    [Required]
    public int UsuarioID { get; set; }

    [Required]
    public int SolicitudID { get; set; }

    [Required]
    [StringLength(200)]
    public string Concepto { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Monto { get; set; }

    [StringLength(15)]
    public string Estado { get; set; } = "Pendiente";

    [Column(TypeName = "decimal(10,2)")]
    public decimal MontoPagado { get; set; } = 0.00m;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal SaldoPendiente { get; private set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("UsuarioID")]
    public virtual Usuario Usuario { get; set; } = null!;

    [ForeignKey("SolicitudID")]
    public virtual SolicitudesServicio Solicitud { get; set; } = null!;
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}