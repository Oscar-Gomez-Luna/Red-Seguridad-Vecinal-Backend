namespace Backend_RSV.Models.Request
{
    public class MarcadorMapaRegisterRequest
    {
        public int? UsuarioID { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Indicador { get; set; } = string.Empty;
        public string? Comentario { get; set; }
    }
}
