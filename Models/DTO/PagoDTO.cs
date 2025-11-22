public class PagoDto
{
    public int PagoID { get; set; }
    public string FolioUnico { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
    public string TipoPago { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }

    public ComprobanteDto? Comprobante { get; set; }
}