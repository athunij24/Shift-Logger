using Microsoft.AspNetCore.Mvc;
using ShiftsApi.Models;
using ShiftsApi.Services;

namespace ShiftsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            var shifts = await _shiftService.GetShiftsAsync();
            return Ok(shifts);
        }

        // GET: api/Shifts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(long id)
        {
            var shift = await _shiftService.GetShiftAsync(id);
            return shift;
        }

        // PUT: api/Shifts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(long id, Shift shift)
        {
            if (await _shiftService.UpdateShiftAsync(id, shift))
            {
                return NoContent();
            }
            return BadRequest();
        }

        // POST: api/Shifts
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(Shift shift)
        {
            var newShift = await _shiftService.PostShiftAsync(shift);
            return CreatedAtAction(nameof(GetShift), new { id = newShift.Id }, newShift);
        }

        // DELETE: api/Shifts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(long id)
        {
            if (await _shiftService.DeleteShiftAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
