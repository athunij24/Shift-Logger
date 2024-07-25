using Microsoft.EntityFrameworkCore;
using ShiftsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftsApi.Data
{
    public interface IEmployeeRepository
    {
        Task<bool> EmployeeExistsAsync(long id);
        Task<Employee> GetEmployeeByIdAsync(long id);
        Task<Employee> GetEmployeeByUserNameAsync(string userName);
        Task<List<Employee>> GetAllEmployeesAsync();
        void AddEmployee(Employee employee);  
        void UpdateEmployee(Employee employee); 
        void DeleteEmployee(Employee employee); 
        Task<bool> SaveChangesAsync();
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ShiftDbContext _context;

        public EmployeeRepository(ShiftDbContext context)
        {
            _context = context;
        }

        // Async because it involves querying the database
        public async Task<bool> EmployeeExistsAsync(long id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        // Async because it involves a database query
        public async Task<Employee> GetEmployeeByIdAsync(long id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // Async because it involves a database query
        public async Task<Employee> GetEmployeeByUserNameAsync(string userName)
        {
            return await _context.Employees
                .SingleOrDefaultAsync(e => e.UserName == userName);
        }

        // Async because it involves a database query
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        // Synchronous because it adds the entity to the DbSet; SaveChangesAsync handles actual DB operations
        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        // Synchronous because it marks the entity as modified; SaveChangesAsync handles actual DB operations
        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        // Synchronous because it removes the entity from the DbSet; SaveChangesAsync handles actual DB operations
        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        // Async because it involves saving changes to the database
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
