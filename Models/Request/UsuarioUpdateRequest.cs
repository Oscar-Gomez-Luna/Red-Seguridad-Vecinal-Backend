namespace Backend_RSV.Models.Request
{
    public class UsuarioUpdateRequest
    {
        public int UsuarioID { get; set; }
        public string? NumeroCasa { get; set; }
        public string? Calle { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string NumeroTarjeta { get; set; } = string.Empty;
        public string FechaVencimiento { get; set; } = string.Empty;
    }
}