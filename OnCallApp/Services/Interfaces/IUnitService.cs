using OnCallApp.Models;

namespace OnCallApp.Services.Interfaces
{
    public interface IUnitService
    {
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<Unit?> GetUnitByIdAsync(int id);
        Task CreateUnitAsync(Unit unit);
        Task UpdateUnitAsync(Unit unit);
        Task DeleteUnitAsync(int id);
    }
}
