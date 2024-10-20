namespace CuentasService.Models
{
    public class MovimientoPutDto
    {
        public int Id { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty; // "Deposito" o "Retiro"
        public decimal Valor { get; set; }
        public int CuentaId { get; set; } // ID de la cuenta asociada (Foreign Key)
    }
}
