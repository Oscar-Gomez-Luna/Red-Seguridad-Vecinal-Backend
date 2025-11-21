using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Usuario
{
    [Key]
    public int UsuarioID { get; set; }

    [Required]
    public int PersonaID { get; set; }

    [Required]
    [StringLength(128)]
    public string FirebaseUID { get; set; } = string.Empty;

    [Required]
    public int TipoUsuarioID { get; set; }

    [StringLength(5)]
    public string? NumeroCasa { get; set; }

    [StringLength(50)]
    public string? Calle { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public bool Activo { get; set; } = true;

    [StringLength(500)]
    public string? MotivoDesactivar { get; set; }

    // Navigation properties
    [ForeignKey("PersonaID")]
    public virtual Persona Persona { get; set; } = null!;

    [ForeignKey("TipoUsuarioID")]
    public virtual TipoUsuario TipoUsuario { get; set; } = null!;

    public virtual ICollection<AlertaPanico> AlertasPanico { get; set; } = new List<AlertaPanico>();
    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
    public virtual ICollection<Aviso> Avisos { get; set; } = new List<Aviso>();
    public virtual ICollection<QRPersonal> QRPersonales { get; set; } = new List<QRPersonal>();
    public virtual ICollection<Invitado> Invitados { get; set; } = new List<Invitado>();
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public virtual ICollection<SolicitudesServicio> SolicitudesServicio { get; set; } = new List<SolicitudesServicio>();
    public virtual CuentaUsuario? CuentaUsuario { get; set; }

    [JsonIgnore]
    public virtual ICollection<CargoMantenimiento> CargosMantenimiento { get; set; } = new List<CargoMantenimiento>();
    [JsonIgnore]
    public virtual ICollection<CargoServicio> CargosServicios { get; set; } = new List<CargoServicio>();
    [JsonIgnore]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public virtual ICollection<MarcadorMapa> MarcadoresMapa { get; set; } = new List<MarcadorMapa>();
}