namespace Backend_RSV.Models.Request
{
    public class MarcadorMapaUpdateRequest
    {
        public int MarcadorID { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Indicador { get; set; } = string.Empty;
        public string? Comentario { get; set; }
    }
}
