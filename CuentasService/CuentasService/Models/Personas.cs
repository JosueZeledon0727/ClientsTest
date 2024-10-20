namespace CuentasService.Models
{
    // DTO FOR ACCESSING/STORING DATA NEEDED TO LINK CUENTA TO PERSONA
    public class Personas
    {
        public int Id { get; set; } // Clave primaria
        public string Nombre { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public int ClienteId { get; set; }
        public string Contrasena { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
