using OnCallApp.Models;
using OnCallApp.Repositories.Interfaces;
using OnCallApp.Services.Interfaces;

namespace OnCallApp.Services.Implementations
{
    public class UnitService : IUnitService
    {
        private readonly IRepository<Unit> _unitRepository;

        public UnitService(IRepository<Unit> unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            // Only get active units? For admin screens, we might need to see all, but let's return all and handle display in view
            return await _unitRepository.GetAllAsync();
        }

        public async Task<Unit?> GetUnitByIdAsync(int id)
        {
            return await _unitRepository.GetAsync(u => u.Id == id);
        }

        public async Task CreateUnitAsync(Unit unit)
        {
            unit.IsActive = true; // Ensure active on creation
            await _unitRepository.CreateAsync(unit);
            await _unitRepository.SaveAsync();
        }

        public async Task UpdateUnitAsync(Unit unit)
        {
            _unitRepository.Update(unit);
            await _unitRepository.SaveAsync();
        }

        public async Task DeleteUnitAsync(int id)
        {
            var unit = await _unitRepository.GetAsync(u => u.Id == id);
            if (unit != null)
            {
                // Soft delete based on document requirement
                unit.IsActive = false;
                _unitRepository.Update(unit);
                await _unitRepository.SaveAsync();
            }
        }
    }
}
