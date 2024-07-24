using Microsoft.EntityFrameworkCore;
using ShiftsApi.Models;

namespace ShiftsApi.Services
{
    public class EmployeeService
    {
        private readonly ShiftDbContext _context;

        public EmployeeService(ShiftDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> RegisterAsync(Employee employee)
        {
            if (await _context.Employees.AnyAsync(e => e.UserName == employee.UserName))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> LoginAsync(string userName, string password)
        {
            var existingEmployee = await _context.Employees
                .SingleOrDefaultAsync(e => e.UserName == userName);

            if (existingEmployee == null || existingEmployee.Password != password)
            {
                return null; // Invalid login
            }

            return existingEmployee; // Successful login
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(long id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<bool> UpdateEmployeeAsync(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                return false;
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteEmployeeAsync(long id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return false; // Employee not found
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool EmployeeExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
