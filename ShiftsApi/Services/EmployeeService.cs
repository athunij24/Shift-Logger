using ShiftsApi.Data;
using ShiftsApi.Models;

namespace ShiftsApi.Services
{
    public interface IEmployeeService
    {
        Task<Employee> RegisterAsync(Employee employee);
        Task<Employee> LoginAsync(string userName, string password);
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeAsync(long id);
        Task<bool> UpdateEmployeeAsync(long id, Employee employee);
        Task<bool> DeleteEmployeeAsync(long id);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> RegisterAsync(Employee employee)
        {
            if (await _employeeRepository.GetEmployeeByUserNameAsync(employee.UserName) != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            _employeeRepository.AddEmployee(employee);
            await _employeeRepository.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> LoginAsync(string userName, string password)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByUserNameAsync(userName);

            if (existingEmployee == null || existingEmployee.Password != password)
            {
                return null; // Invalid login
            }

            return existingEmployee; // Successful login
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee> GetEmployeeAsync(long id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<bool> UpdateEmployeeAsync(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                return false;
            }

            if (!await _employeeRepository.EmployeeExistsAsync(id))
            {
                return false;
            }

            _employeeRepository.UpdateEmployee(employee);
            return await _employeeRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteEmployeeAsync(long id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return false; // Employee not found
            }

            _employeeRepository.DeleteEmployee(employee);
            return await _employeeRepository.SaveChangesAsync();
        }
    }

}
