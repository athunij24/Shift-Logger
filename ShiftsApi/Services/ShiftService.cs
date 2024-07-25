using Microsoft.AspNetCore.Mvc;
using ShiftsApi.Data;
using ShiftsApi.Models;

namespace ShiftsApi.Services
{
    public interface IShiftService
    {
        Task<List<Shift>> GetShiftsAsync();
        Task<ActionResult<Shift>> GetShiftAsync(long id);
        Task<bool> UpdateShiftAsync(long id, Shift shift);
        Task<Shift> PostShiftAsync(Shift shift);
        Task<bool> DeleteShiftAsync(long id);
    }
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftService(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public async Task<List<Shift>> GetShiftsAsync()
        {
            return await _shiftRepository.GetShiftsAsync();
        }

        public async Task<ActionResult<Shift>> GetShiftAsync(long id)
        {
            var shift = await _shiftRepository.GetShiftAsync(id);
            if (shift == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(shift);
        }

        public async Task<bool> UpdateShiftAsync(long id, Shift shift)
        {
            if (id != shift.Id)
            {
                return false;
            }

            return await _shiftRepository.UpdateShiftAsync(shift);
        }

        public async Task<Shift> PostShiftAsync(Shift shift)
        {
            return await _shiftRepository.PostShiftAsync(shift);
        }

        public async Task<bool> DeleteShiftAsync(long id)
        {
            return await _shiftRepository.DeleteShiftAsync(id);
        }
    }
}
