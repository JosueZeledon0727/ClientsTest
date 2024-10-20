using CuentasService.Models;

namespace CuentasService.Repositories
{
    public interface ICuentaRepository
    {
        Task<IEnumerable<Cuenta>> GetAllAsync();
        Task<Cuenta> GetByIdAsync(int id);
        Task AddAsync(Cuenta cuenta);
        Task UpdateAsync(Cuenta cuenta);
        Task DeleteAsync(int id);
        Task<bool> CuentaExistsAsync(int id);
    }
}
