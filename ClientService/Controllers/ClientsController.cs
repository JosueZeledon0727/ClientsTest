using ClientService.Models;
using ClientService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IClienteRepository _clienteRepository;

        public ClientsController(DataContext context, IClienteRepository clienteRepository)
        {
            _context = context;
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var clients = new List<Cliente>
            //{
            //    new Cliente
            //    {
            //        Id = 1,
            //        Nombre = "Raul",
            //        Edad = 18,
            //        ClienteId = 1,
            //        Contrasena = "1234",
            //        Estado = true
            //    }
            //};
            //return Ok(clients);
            // Obtener todos los clientes de la tabla Personas filtrando por Discriminator
            //var clientes = await _context.Clientes.ToListAsync(); // Esto ya filtra automáticamente por el Discriminator

            //return Ok(clientes);

            var clientes = await _clienteRepository.GetAllAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var client = await _clienteRepository.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet("clientId/{clientId}")]
        public async Task<ActionResult<Cliente>> GetClienteByClientID(int clientId)
        {
            //Console.WriteLine("Valor es: " + clientId);
            var client = await _clienteRepository.GetByClientIdAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            // Verificar si el ClienteID ya existe
            var clients = await _clienteRepository.GetAllAsync();
            var existingClient = clients.FirstOrDefault(c => c.ClienteId == cliente.ClienteId);

            if (existingClient != null)
            {
                return Conflict("El ClienteID que intentas usar ya existe."); // Retorna conflicto si ya existe
            }

            // Crear la entidad base Persona y asignar los valores
            //cliente.Discriminator = "Cliente"; // Asegúrate de establecer el Discriminator
            try
            {
                // Agregar el cliente al contexto
                await _clienteRepository.AddAsync(cliente);
                await _clienteRepository.SaveChangesAsync(); // Guardar en la base de datos

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Cliente>> UpdateCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("Client ID no coincide");
            }

            var clients = await _clienteRepository.GetAllAsync();
            var existingCurrentClient = clients.FirstOrDefault(x => x.ClienteId == cliente.ClienteId && x.Id != id);

            if (existingCurrentClient != null)
            {
                return Conflict("El ClienteID que intentas usar ya existe.");
            }
            try
            {
                await _clienteRepository.UpdateAsync(cliente);  // Updates client to EntityState.Modified
                await _clienteRepository.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Returning 204 for appropiate return code
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Cliente>> PartialUpdateCliente(int id, [FromBody] JsonPatchDocument<Cliente> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var client = await _clienteRepository.GetByIdAsync(id);

            if (client == null)
            {
                // Client does not exist
                return NotFound();
            }

            patchDoc.ApplyTo(client, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Checks if ClientID is unique
            var clients = await _clienteRepository.GetAllAsync();
            var existingCurrentClient = clients.FirstOrDefault(x => x.ClienteId == client.ClienteId && x.Id != client.Id);

            if (existingCurrentClient != null)
                return Conflict("El ClienteId ya existe.");

            try
            {
                await _clienteRepository.SaveChangesAsync(); // Guarda los cambios en la base de datos
            }
            catch (DbUpdateException)
            {
                var clientExists = await _clienteRepository.GetByIdAsync(id);

                if (clientExists == null)
                {
                    return NotFound(); // Returning 404 if client does not exist
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando cliente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Returning 204 for appropiate return code
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(int id)
        {
            try
            {
                await _clienteRepository.DeleteAsync(id);   // Uses repository DeleteAsync method
                await _clienteRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Client.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Returning 204 as successfull delete was made
        }
    }
}
