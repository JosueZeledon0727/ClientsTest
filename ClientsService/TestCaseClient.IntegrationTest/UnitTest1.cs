using ClientService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace TestCaseClient.IntegrationTest
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {

            Random rng = new Random();
            int randClientId = rng.Next(100); // number between 0 and 99

            // Arrange
            var nuevoCliente = new Cliente
            {
                Nombre = "Juan Pérez",
                Genero = "Masculino",
                Edad = 30,
                Identificacion = "12345678",
                Direccion = "Calle Falsa 123",
                Telefono = "987654321",
                ClienteId = randClientId,
                Contrasena = "password",
                Estado = true
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/clients", nuevoCliente);

            // Assert
            response.EnsureSuccessStatusCode();
            var clienteCreado = await response.Content.ReadFromJsonAsync<Cliente>();
            Assert.NotNull(clienteCreado);
            Assert.Equal(nuevoCliente.Nombre, clienteCreado.Nombre);
        }
    }
}