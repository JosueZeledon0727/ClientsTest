namespace CuentasService.Models
{
    public class Cuenta
    {
        public int Id { get; set; } // PK
        public string NumeroCuenta { get; set; } = string.Empty; // Clave única
        public string TipoCuenta { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }

        // Clave foránea para relacionar con Persona
        public int ClienteId { get; set; }
        //public Personas Cliente { get; set; } // Navegación a Persona
    }
}
