using ClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Data
{
    public class DataContext : DbContext
    {
        //public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la tabla de Personas
            modelBuilder.Entity<Persona>()
                .ToTable("Personas")
                .HasKey(p => p.Id); // Configura la clave primaria en Persona

            // Configurar la tabla de Clientes
            modelBuilder.Entity<Cliente>()
                .ToTable("Personas") // Ambas se almacenan en la misma tabla
                .Property(c => c.ClienteId) // Asegúrate de que el nombre sea correcto
                .IsRequired(); // Asegúrate de que ClienteID sea requerido

            // Configurar el campo Estado
            modelBuilder.Entity<Cliente>()
                .Property(c => c.Estado)
                .HasColumnType("BIT"); // Campo booleano para el estado

            // Definir el Discriminator en la clase base
            modelBuilder.Entity<Persona>()
                .Property<string>("Discriminator") // Columna para discriminar tipos
                .HasDefaultValue("Persona"); // Valor por defecto para Persona

            // Definir el valor de Discriminator para Cliente
            modelBuilder.Entity<Cliente>()
                .Property<string>("Discriminator")
                .HasDefaultValue("Cliente"); // Valor por defecto para Cliente

            // Agregar índice único en ClienteID
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.ClienteId)
                .IsUnique(); // Hacer que el índice sea único
        }

    }
}
