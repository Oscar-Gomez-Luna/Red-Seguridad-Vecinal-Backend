using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CuentaUsuario
{
    [Key]
    public int CuentaID { get; set; }

    [Required]
    public int UsuarioID { get; set; }

    [Required]
    public byte[] NumeroTarjeta { get; set; } = null!;

    [Required]
    [StringLength(4)]
    public string UltimosDigitos { get; set; } = string.Empty;

    [Required]
    public string FechaVencimiento { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal SaldoMantenimiento { get; set; } = 0.00m;

    [Column(TypeName = "decimal(10,2)")]
    public decimal SaldoServicios { get; set; } = 0.00m;

    [Column(TypeName = "decimal(10,2)")]
    public decimal SaldoTotal { get; set; } = 0.00m;

    public DateTime UltimaActualizacion { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("UsuarioID")]
    public virtual Usuario Usuario { get; set; } = null!;
}