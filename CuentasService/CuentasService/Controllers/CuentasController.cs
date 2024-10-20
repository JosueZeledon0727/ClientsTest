using CuentasService.Data;
using CuentasService.Models;
using CuentasService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.XPath;

namespace CuentasService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ICuentaRepository _cuentaRepository;
        private readonly HttpClient _httpClient;

        public CuentasController(DataContext context, ICuentaRepository cuentaRepository, HttpClient httpClient)
        {
            _context = context;
            _cuentaRepository = cuentaRepository;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas()
        {
            // Obtener todas las cuentas
            var cuentas = await _cuentaRepository.GetAllAsync();
            return Ok(cuentas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetCuentaById(int id)
        {
            var cuenta = await _cuentaRepository.GetByIdAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return cuenta;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta(Cuenta cuentaDto)
        {
            // Verificar si el ClienteID existe llamando al microservicio de Cliente
            var response = await _httpClient.GetAsync($"https://localhost:7123/api/Clients/clientId/{cuentaDto.ClienteId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound($"Cliente con ID {cuentaDto.ClienteId} no encontrado.");
            }

            await _cuentaRepository.AddAsync(cuentaDto);

            // Guardar cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Manejo de excepciones al guardar
                return BadRequest($"Error al crear la cuenta: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetCuentaById), new { id = cuentaDto.Id }, cuentaDto);
        }

        // PUT: api/cuentas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id)
            {
                return BadRequest("ID de Cuenta no existe");
            }

            // Verificar si el ClienteID que quiero actualizar existe llamando al microservicio de Cliente
            var response = await _httpClient.GetAsync($"https://localhost:7123/api/Clients/clientId/{cuenta.ClienteId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound($"Cliente con ID {cuenta.ClienteId} no encontrado.");
            }

            _context.Entry(cuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Returns 204 otherwise
        }

        
        [HttpPatch("{id}")]
        public async Task<ActionResult<Cuenta>> PartialUpdateCuenta(int id, [FromBody] JsonPatchDocument<Cuenta> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("El patchDoc no puede ser nulo.");
            }

            var cuenta = await _context.Cuentas.FindAsync(id);

            if (cuenta == null)
            {
                // Cuenta does not exist
                return NotFound();
            }

            patchDoc.ApplyTo(cuenta, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si ClienteId fue actualizado y si existe en la base de datos
            if (ModelState.ContainsKey("ClienteId"))
            {
                var clienteId = cuenta.ClienteId; // Obtener el ClienteId actualizado

                var response = await _httpClient.GetAsync($"https://localhost:7123/api/Clients/clientId/{cuenta.ClienteId}");

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest($"Cliente con ID {cuenta.ClienteId} no existe.");
                }
            }

            try
            {
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error on updating Client: {ex.Message}");
            }

            return NoContent();


        }




        // DELETE: api/cuentas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound($"Cuenta con Id {id} no encontrada.");
            }

            await _cuentaRepository.DeleteAsync(id);
            return NoContent();

        }


        // REPORTE SOLICITADO DE MOVIMIENTOS
        [HttpGet("reportes")]
        public async Task<ActionResult> GetEstadoCuenta([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, [FromQuery] int clienteId)
        {
            // Verificar si el ClienteID existe llamando al microservicio de Cliente
            var response = await _httpClient.GetAsync($"https://localhost:7123/api/Clients/clientId/{clienteId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound($"Cliente con ID {clienteId} no encontrado.");
            }

            var clienteJson = await response.Content.ReadAsStringAsync();
            var cliente = JsonConvert.DeserializeObject<Personas>(clienteJson);


            // Verificar si el cliente existe
            var cuentas = await _context.Cuentas
                        .Where(c => c.ClienteId == clienteId)
                        .ToListAsync();

            // Chequeamos si no hay cuentas asociadas
            if (cuentas == null || !cuentas.Any())
            {
                return NotFound("No se encontraron cuentas para el cliente.");
            }

            // Recuperamos los movimientos del cliente en el rango de fechas
            var movimientos = await _context.Movimientos
                .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin
                             && cuentas.Select(c => c.Id).Contains(m.CuentaId))
                .ToListAsync();

            // Creamos el objeto de respuesta
            var respuesta = new
            {
                ClienteId = clienteId,
                NombreCliente = cliente.Nombre,
                Cuentas = cuentas.Select(c => new
                {
                    c.NumeroCuenta,
                    Saldo = c.SaldoInicial,
                    Movimientos = movimientos
                        .Where(m => m.CuentaId == c.Id)
                        .Select(m => new
                        {
                            m.Fecha,
                            m.TipoMovimiento,
                            m.Valor,
                            m.Saldo
                        }).ToList()
                })
                .Where(c => c.Movimientos.Any()) // Filtramos las cuentas sin movimientos
                .ToList()
            };


            // Chequemaos si no hay cuentas con movimientos
            if (!respuesta.Cuentas.Any())
            {
                return NotFound("No se encontraron movimientos para las cuentas del cliente en el rango de fechas especificado.");
            }

            return Ok(respuesta);
        }

    }
}
