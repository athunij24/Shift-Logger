using Microsoft.AspNetCore.Mvc;
using ShiftsApi.Models;
using ShiftsApi.Services;

namespace ShiftsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftService _shiftService;

        public ShiftsController(ShiftService service)
        {
            _shiftService = service;
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            return await _shiftService.GetShiftsAsync();
        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(long id)
        {
            var shift = await _shiftService.GetShiftAsync(id);

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
            bool success = await _shiftService.UpdateShiftAsync(id, shift);
            if (!success)
            {
                return BadRequest(new { message = "Invalid shift ID or mismatch with provided data." });
            }

            return NoContent(); // Return 204 No Content if update was successful
        }

        // POST: api/Shifts
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(Shift shift)
        {
            try
            {
                var newShift = await _shiftService.PostShiftAsync(shift);
                return CreatedAtAction(nameof(GetShift), new { id = newShift.Id }, newShift);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(long id)
        {
            bool success = await _shiftService.DeleteShiftAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"Shift with ID {id} not found." });
            }

            return NoContent(); // Return 204 No Content if deletion was successful
        }


    }
}
