namespace Backend_RSV.Models.Request
{
    public class AvisoRegistroRequest
    {
        public int UsuarioID { get; set; }
        public int CategoriaID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime? FechaEvento { get; set; }
    }
}