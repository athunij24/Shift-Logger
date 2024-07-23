using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsApi.Models;

namespace ShiftsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftDbContext _context;

        public ShiftsController(ShiftDbContext context)
        {
            _context = context;
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            return await _context.Shifts.ToListAsync();
        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(long id)
        {
            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
            {
                return NotFound(new { message = $"Shift with ID {id} not found." });
            }

            return shift;
        }

        // PUT: api/Shifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(long id, Shift shift)
        {
            if (id != shift.Id)
            {
                return BadRequest(new { message = "Shift ID mismatch." });
            }

            _context.Entry(shift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
                {
                    return NotFound(new { message = $"Shift with ID {id} does not exist." });
                }
                else
                {
                    throw; // Will be caught by the middleware
                }
            }

            return NoContent();
        }

        // POST: api/Shifts
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(Shift shift)
        {
            if (shift.StartTime >= shift.EndTime)
            {
                return BadRequest(new { message = "Shift start time must be before end time." });
            }

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShift), new { id = shift.Id }, shift);
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(long id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound(new { message = $"Shift with ID {id} not found." });
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShiftExists(long id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
