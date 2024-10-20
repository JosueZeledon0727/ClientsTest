using CuentasService.Models;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Data
{
    public class DataContext : DbContext
    { 
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Configuración para la tabla existente Personas
            modelBuilder.Entity<Personas>()
                .ToTable("Personas"); // La tabla Personas ya existe!

            // Configuración para la relación entre Cuenta y Persona
            modelBuilder.Entity<Cuenta>()
                .HasOne<Personas>() // Se refiere a la entidad Persona
                .WithMany() // Cada Persona puede tener muchas Cuentas
                .HasForeignKey(c => c.ClienteId) // ClienteId es la clave foránea
                .HasPrincipalKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); // Comportamiento en caso de eliminación

            // Configuración para el Saldo Inicial
            modelBuilder.Entity<Cuenta>()
                .Property(c => c.SaldoInicial)
                .HasColumnType("decimal(18,2)");

            // Configurar el campo Estado para Cuenta
            modelBuilder.Entity<Cuenta>()
                .Property(c => c.Estado)
                .HasColumnType("BIT"); // Campo booleano para el estado



            // Configuración para la entidad Movimiento
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Cuenta) // Relación con Cuenta
                .WithMany() // Una Cuenta puede tener muchos Movimientos
                .HasForeignKey(m => m.CuentaId) // CuentaId es la clave foránea
                .OnDelete(DeleteBehavior.Cascade); // Comportamiento en caso de eliminación

            // Configuración para el Valor
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Valor)
                .HasColumnType("decimal(18,2)");

            // Configuración para el Saldo
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Saldo)
                .HasColumnType("decimal(18,2)");
        }

    }
}
