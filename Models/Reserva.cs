using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reserva
{
    [Key]
    public int ReservaID { get; set; }

    [Required]
    public int UsuarioID { get; set; }

    [Required]
    public int AmenidadID { get; set; }

    [Required]
    public DateOnly FechaReserva { get; set; }

    [Required]
    public TimeSpan HoraInicio { get; set; }

    [Required]
    public TimeSpan HoraFin { get; set; }

    [StringLength(500)]
    public string? Motivo { get; set; }

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Pendiente";

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("UsuarioID")]

    public virtual Usuario Usuario { get; set; } = null!;

    [ForeignKey("AmenidadID")]
    public virtual Amenidad Amenidad { get; set; } = null!;
}