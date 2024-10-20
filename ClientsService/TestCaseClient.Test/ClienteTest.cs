using ClientService.Models;
using FluentAssertions;

namespace TestCaseClient.Test
{
    public class ClienteTest
    {
        [Fact]
        public void Cliente_CreateNewClient()
        {
            // Arrange
            var cliente = new Cliente
            {
                ClienteId = 1,
                Nombre = "Juan Perez",
                Genero = "Masculino",
                Edad = 30,
                Identificacion = "123456789",
                Direccion = "Calle Falsa 123",
                Telefono = "987654321",
                Contrasena = "ContraseñaSegura",
                Estado = true
            };

            // Act & Assert
            cliente.ClienteId.Should().Be(1);
            cliente.Nombre.Should().Be("Juan Perez");
            cliente.Genero.Should().Be("Masculino");
            cliente.Edad.Should().Be(30);
            cliente.Identificacion.Should().Be("123456789");
            cliente.Direccion.Should().Be("Calle Falsa 123");
            cliente.Telefono.Should().Be("987654321");
            cliente.Contrasena.Should().Be("ContraseñaSegura");
            cliente.Estado.Should().Be(true);
        }

        [Fact]
        public void Cliente_PropiedadesNoPuedenSerNulas()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nombre = "Nombre de Prueba", 
                Contrasena = "ContrasenaSegura"
            };

            // Act & Assert
            cliente.Nombre.Should().NotBeNullOrEmpty();
            cliente.Contrasena.Should().NotBeNullOrEmpty();
        }
    }
}