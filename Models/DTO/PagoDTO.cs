public class PagoDto
{
    public int PagoID { get; set; }
    public string FolioUnico { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
    public string TipoPago { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
    public string? UltimosDigitosTarjeta { get; set; }

    public UsuarioDto? Usuario { get; set; }
    public List<DetallePagoDto> DetallesPago { get; set; } = new();
    public ComprobanteDto? Comprobante { get; set; }
}

public class UsuarioDto
{
    public int UsuarioID { get; set; }
    public string Correo { get; set; } = string.Empty;

    public PersonaDto? Persona { get; set; }
}

public class PersonaDto
{
    public int PersonaID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
}

public class DetallePagoDto
{
    public int DetalleID { get; set; }
    public decimal MontoAplicado { get; set; }
    public DateTime FechaAplicacion { get; set; }
    public int? CargoMantenimientoID { get; set; }
    public int? CargoServicioID { get; set; }
}
