using Microsoft.AspNetCore.Mvc;
using ShiftsApi.Models;
using ShiftsApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // POST: api/Employees/Register
        [HttpPost("register")]
        public async Task<ActionResult<Employee>> Register(Employee employee)
        {
            try
            {
                var newEmployee = await _employeeService.RegisterAsync(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.Id }, newEmployee);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Employees/Login
        [HttpPost("login")]
        public async Task<ActionResult<Employee>> Login(string userName, string password)
        {
            var employee = await _employeeService.LoginAsync(userName, password);

            if (employee == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(employee);
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/Employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Ok(employee);
        }

        // PUT: api/Employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest(new { message = "Employee ID mismatch." });
            }

            if (!await _employeeService.UpdateEmployeeAsync(id, employee))
            {
                return NotFound(new { message = "Employee not found." });
            }

            return NoContent();
        }

        // DELETE: api/Employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            if (!await _employeeService.DeleteEmployeeAsync(id))
            {
                return NotFound(new { message = "Employee not found." });
            }

            return NoContent();
        }
    }
}
