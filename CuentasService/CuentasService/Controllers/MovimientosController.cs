using CuentasService.Data;
using CuentasService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;

        public MovimientosController(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientos()
        {
            var movimientos = await _context.Movimientos.ToListAsync();
            return Ok(movimientos);
        }

        // GET MOVIMIENTO POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Movimiento>> GetMovimiento(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return movimiento;
        }

        // POST MOVIMIENTO
        [HttpPost]
        public async Task<ActionResult<Movimiento>> PostMovimiento(MovimientoCreateDto movimiento)
        {
            // Validando primero si la cuenta asociada al intento del post, existe
            var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);

            if (cuenta == null)
            {
                return BadRequest($"La cuenta con ID '{movimiento.CuentaId}' no existe.");
            }

            // Hay que actualizar el saldo, dependiendo del movimiento (Deposito o Retiro)
            if (movimiento.TipoMovimiento == "Deposito")
            {
                cuenta.SaldoInicial += movimiento.Valor; // Actualiza el saldo inicial de la cuenta
            }
            else if (movimiento.TipoMovimiento == "Retiro")
            {
                if (cuenta.SaldoInicial < movimiento.Valor)
                {
                    return BadRequest("Saldo no disponible para el retiro.");
                }
                cuenta.SaldoInicial -= movimiento.Valor; // Actualiza el saldo inicial de la cuenta
            }
            else
            {
                return BadRequest("Tipo de movimiento no válido.");
            }


            // Creamos la instancia de la clase Movimiento, con la fecha de hoy
            var movimientoResult = new Movimiento
            {
                Fecha = DateTime.UtcNow.AddDays(-2),
                TipoMovimiento = movimiento.TipoMovimiento,
                Valor = movimiento.Valor,
                Saldo = cuenta.SaldoInicial,
                CuentaId = movimiento.CuentaId
            };

            // Agregar el movimiento a la base de datos
            try
            {
                _context.Movimientos.Add(movimientoResult);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Excepcion al guardar el movimiento
                return StatusCode(500, $"Error al guardar el movimiento: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetMovimiento), new { id = movimientoResult.Id }, movimientoResult);
        }



        [HttpPut]
        public async Task<ActionResult> UpdateMovimiento(int id, MovimientoPutDto movimiento)
        {
            if (id != movimiento.Id)
            {
                return BadRequest("El ID del movimiento no coincide con el movimiento a actualizar.");
            }

            var movimientoTemp = await _context.Movimientos.FindAsync(id);

            if (movimientoTemp == null)
            {
                return BadRequest($"Movimiento con ID {id} no existe.");
            }

            // Guardamos el TipoMovimiento original y su valor por si fuese modificado
            var tipoMovimientoOriginal = movimientoTemp.TipoMovimiento;
            var valorOriginal = movimientoTemp.Valor;

            // Traemos la cuenta ligada al movimiento
            var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);

            if (cuenta == null)
            {
                return NotFound("Cuenta no encontrada.");
            }

            // Revertir el saldo según el tipo de movimiento original
            if (tipoMovimientoOriginal == "Deposito")
            {
                cuenta.SaldoInicial -= valorOriginal;
            }
            else if (tipoMovimientoOriginal == "Retiro")
            {
                cuenta.SaldoInicial += valorOriginal;
            }

            // Actualizar los valores del movimiento
            movimientoTemp.TipoMovimiento = movimiento.TipoMovimiento;
            movimientoTemp.Valor = movimiento.Valor;

            // Ahora, validamos el nuevo tipo de movimiento
            if (movimientoTemp.TipoMovimiento == "Deposito")
            {
                cuenta.SaldoInicial += movimientoTemp.Valor;
            }
            else if (movimientoTemp.TipoMovimiento == "Retiro")
            {
                if (cuenta.SaldoInicial < movimientoTemp.Valor)
                {
                    return BadRequest("No se puede reintegrar/eliminar el deposito debido a que la cuenta no tiene suficiente saldo para reponerlo.");
                }
                cuenta.SaldoInicial -= movimientoTemp.Valor; // Actualizamos el saldo debido al retiro 
            }
            else
            {
                return BadRequest("El tipo de movimiento a usar no es válido. Debe ser 'Retiro' o 'Deposito'.");
            }

            // Actualizamos el saldo del movimiento
            movimientoTemp.Saldo = cuenta.SaldoInicial;

            // Guardamos cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el movimiento: {ex.Message}");
            }

            return NoContent();

        }


        [HttpPatch]
        public async Task<ActionResult<Movimiento>> PartialUpdateMovimiento(int id, [FromBody] JsonPatchDocument<Movimiento> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("El patchDoc no puede ser nulo.");
            }

            var movimiento = await _context.Movimientos.FindAsync(id);

            if (movimiento == null)
            {
                return BadRequest($"Movimiento con ID {id} no existe.");
            }

            // Guardamos el TipoMovimiento original y su valor por si fuese modificado
            var tipoMovimientoOriginal = movimiento.TipoMovimiento;
            var valorOriginal = movimiento.Valor;

            // Aplicamos los cambios
            patchDoc.ApplyTo(movimiento, ModelState);

            // Validar el modelo después de aplicar el parche
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Traemos la cuenta ligada al movimiento
            var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);

            if (cuenta == null)
            {
                return NotFound("Cuenta no encontrada.");
            }

            // Revertir el saldo según el tipo de movimiento original
            if (tipoMovimientoOriginal == "Deposito")
            {
                cuenta.SaldoInicial -= valorOriginal; 
            }
            else if (tipoMovimientoOriginal == "Retiro")
            {
                cuenta.SaldoInicial += valorOriginal;
            }

            // Ahora, validamos el nuevo tipo de movimiento
            if (movimiento.TipoMovimiento == "Deposito")
            {
                cuenta.SaldoInicial += movimiento.Valor; 
            }
            else if (movimiento.TipoMovimiento == "Retiro")
            {
                if (cuenta.SaldoInicial < movimiento.Valor)
                {
                    return BadRequest("No se puede reintegrar/eliminar el deposito debido a que la cuenta no tiene suficiente saldo para reponerlo.");
                }
                cuenta.SaldoInicial -= movimiento.Valor; // Actualizamos el saldo debido al retiro 
            }
            else
            {
                return BadRequest("El tipo de movimiento a usar no es válido. Debe ser 'Retiro' o 'Deposito'.");
            }


            movimiento.Saldo = cuenta.SaldoInicial;

            // Guardamos los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el movimiento: {ex.Message}");
            }

            return Ok(movimiento); // Retornamos el movimiento actualizado

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMovimiento(int id)
        {
            // Chequemaos por el movimiento a ver si existe con el Id
            var movimiento = await _context.Movimientos.FindAsync(id);

            if(movimiento == null)
            {
                return BadRequest($"Movimiento con ID {id} no existe.");
            }

            // Recuperamos la cuenta ligada al movimiento para revertir las operaciones del movimiento
            var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);

            if(cuenta != null)
            {
                // Chequeamos si el movimiento es un deposito, ya que en ese caso hay que revertir la operacion y verificar si el saldo actual
                // de la cuenta lo permite, por ejemplo, cuando la cuenta quedo en 100 y el movimiento fue un deposito de 200, ahi seria invalido eliminarlo para no tener la cuenta en -100
                if (movimiento.TipoMovimiento == "Deposito")
                {
                    if(cuenta.SaldoInicial < movimiento.Valor)
                    {
                        return BadRequest("No se puede reintegrar/eliminar el deposito debido a que la cuenta no tiene suficiente saldo para reponerlo.");
                    }
                    else
                    {
                        cuenta.SaldoInicial -= movimiento.Valor;
                    }
                }
                else if (movimiento.TipoMovimiento == "Retiro")
                {
                    cuenta.SaldoInicial += movimiento.Valor;
                }
            }

            // Eliminamos el movimiento
            try
            {
                _context.Movimientos.Remove(movimiento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al eliminar el movimiento: {ex.Message}");
            }

            return NoContent();

        }
    }
}
