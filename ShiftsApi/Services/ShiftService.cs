using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsApi.Models;

namespace ShiftsApi.Services
{
    public class ShiftService
    {
        private readonly ShiftDbContext _context;

        public ShiftService(ShiftDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shift>> GetShiftsAsync()
        {
            return await _context.Shifts.ToListAsync();
        }

        public async Task<ActionResult<Shift>> GetShiftAsync(long id)
        {
            return await _context.Shifts.FindAsync(id);
        }

        public async Task<bool> UpdateShiftAsync(long id, Shift shift)
        {
            if (id != shift.Id)
            {
                return false;
            }

            _context.Entry(shift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
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


        private bool ShiftExists(long id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
