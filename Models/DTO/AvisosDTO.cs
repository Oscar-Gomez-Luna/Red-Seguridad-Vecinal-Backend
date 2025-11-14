public class AvisosDTO
{
    public int AvisoID { get; set; }
    public int UsuarioID { get; set; }
    public int CategoriaID { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaEvento { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public bool CategoriaActiva { get; set; }
}
