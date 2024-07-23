using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsApi.Models;

namespace ShiftsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ShiftDbContext _context;

        public EmployeesController(ShiftDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            return employee;
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest(new { message = "Employee ID mismatch." });
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound(new { message = $"Employee with ID {id} does not exist." });
                }
                else
                {
                    throw; 
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/Employees/register
        [HttpPost("register")]
        public async Task<ActionResult<Employee>> RegisterEmployee(Employee employee)
        {
            if (await _context.Employees.AnyAsync(e => e.UserName == employee.UserName))
            {
                return Conflict(new { message = "Username is already in use." });
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // Get: api/Employees/login
        [HttpPost("login")]
        public async Task<ActionResult<Employee>> LoginEmployee(Employee employee)
        {
            var existingEmployee = await _context.Employees
                .SingleOrDefaultAsync(e => e.UserName == employee.UserName);

            if (existingEmployee == null)
            {
                return NotFound(new { message = "Username not found." });
            }

            if (existingEmployee.Password != employee.Password)
            {
                return Conflict(new { message = "Incorrect password." });
            }

            return Ok(existingEmployee);
        }

        private bool EmployeeExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
