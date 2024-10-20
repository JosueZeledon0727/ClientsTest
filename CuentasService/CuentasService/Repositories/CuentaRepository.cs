using CuentasService.Data;
using CuentasService.Models;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly DataContext _context;

        public CuentaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cuenta>> GetAllAsync()
        {
            return await _context.Cuentas.ToListAsync();
        }

        public async Task<Cuenta> GetByIdAsync(int id)
        {
            return await _context.Cuentas.FindAsync(id);
        }

        public async Task AddAsync(Cuenta cuenta)
        {
            await _context.Cuentas.AddAsync(cuenta);
        }

        public async Task UpdateAsync(Cuenta cuenta)
        {
            _context.Entry(cuenta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cuenta = await GetByIdAsync(id);
            if (cuenta != null)
            {
                _context.Cuentas.Remove(cuenta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CuentaExistsAsync(int id)
        {
            return await _context.Cuentas.AnyAsync(c => c.Id == id);
        }
    }
}
