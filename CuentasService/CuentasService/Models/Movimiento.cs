namespace CuentasService.Models
{
    public class Movimiento
    {
        public int Id { get; set; } // Clave primaria
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }

        public int CuentaId { get; set; } // Clave foránea hacia Cuenta
        public Cuenta Cuenta { get; set; }
    }
}
