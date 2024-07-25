using Microsoft.EntityFrameworkCore;
using ShiftsApi.Models;

namespace ShiftsApi.Data
{
    public interface IShiftRepository
    {
        Task<List<Shift>> GetShiftsAsync();
        Task<Shift?> GetShiftAsync(long id);
        Task<bool> UpdateShiftAsync(Shift shift);
        Task<Shift> PostShiftAsync(Shift shift);
        Task<bool> DeleteShiftAsync(long id);
        Task<bool> ShiftExistsAsync(long id);
    }
    public class ShiftRepository : IShiftRepository
    {
        private readonly ShiftDbContext _context;

        public ShiftRepository(ShiftDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shift>> GetShiftsAsync()
        {
            return await _context.Shifts.ToListAsync();
        }

        public async Task<Shift?> GetShiftAsync(long id)
        {
            return await _context.Shifts.FindAsync(id);
        }

        public async Task<bool> UpdateShiftAsync(Shift shift)
        {
            _context.Entry(shift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ShiftExistsAsync(shift.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Shift> PostShiftAsync(Shift shift)
        {
            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();
            return shift;
        }

        public async Task<bool> DeleteShiftAsync(long id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return false;
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ShiftExistsAsync(long id)
        {
            return await _context.Shifts.AnyAsync(e => e.Id == id);
        }
    }
}
