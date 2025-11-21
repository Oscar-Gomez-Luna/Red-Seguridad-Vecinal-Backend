namespace Backend_RSV.Models.Request
{
    public class AvisoUpdateRequest
    {
        public int AvisoID { get; set; }
        public int CategoriaID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime? FechaEvento { get; set; }
    }
}