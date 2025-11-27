using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ComprobantePago
{
    [Key]
    public int ComprobanteID { get; set; }

    [Required]
    public int PagoID { get; set; }

    [Required]
    [StringLength(255)]
    public string NombreArchivo { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string TipoArchivo { get; set; } = "application/pdf";

    [Required]
    public byte[] Archivo { get; set; } = null!;

    public DateTime FechaSubida { get; set; } = DateTime.Now;

    [ForeignKey("PagoID")]
    public virtual Pago Pago { get; set; } = null!;
}
