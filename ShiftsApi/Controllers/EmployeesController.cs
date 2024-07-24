using Microsoft.AspNetCore.Mvc;
using ShiftsApi.Models;
using ShiftsApi.Services;

namespace ShiftsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees); // Return 200 OK with the list of employees
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, Employee employee)
        {
            bool success = await _employeeService.UpdateEmployeeAsync(id, employee);
            if (!success)
            {
                return BadRequest(new { message = "Invalid employee ID or mismatch with provided data." });
            }

            return NoContent(); // Return 204 No Content if update was successful
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                var newEmployee = await _employeeService.RegisterAsync(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.Id }, newEmployee);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            bool success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }

            return NoContent(); // Return 204 No Content if deletion was successful
        }

        // POST: api/Employees/login
        [HttpPost("login")]
        public async Task<ActionResult<Employee>> LoginEmployee([FromBody] ShiftsApi.Models.LoginRequest loginRequest)
        {
            var employee = await _employeeService.LoginAsync(loginRequest.Username, loginRequest.Password);
            if (employee == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(employee);
        }
    }
}
