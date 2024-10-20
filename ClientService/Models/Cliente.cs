namespace ClientService.Models
{
    public class Cliente: Persona
    {
        public int ClienteId { get; set; } // Clave única (PK) para el cliente a solicitud de los requerimientos
        public string Contrasena { get; set; }
        public bool Estado { get; set; }
    }
}
