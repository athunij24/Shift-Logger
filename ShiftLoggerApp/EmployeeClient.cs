using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShiftsApi.Models;
using Spectre.Console;

namespace ShiftLoggerApp
{
    public class EmployeeClient
    {
        public async Task RegisterEmployeeAsync()
        {
            var name = AnsiConsole.Ask<string>("Name: ");
            var userName = AnsiConsole.Ask<string>("Username: ");
            var password = AnsiConsole.Ask<string>("Password: ");
            var role = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Role: ")
                .PageSize(10)
                .AddChoices(new[] {
                    "Manager", "Employee", "Region Manager"
                }));
            Employee employee = new Employee
            {
                Name = name, UserName = userName, Password = password, Role = role
            };
            var employeeManager = new EmployeeManager(new HttpClient { BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiEmployeesUrl")) });
            var result = await employeeManager.RegisterEmployeeAsync(employee);
            if(result)
            {
                Console.WriteLine("Registration Success");
            }
            else
            {
                Console.WriteLine("Registration failed");
            }

        }

        public async Task<Employee> Login()
        {
            var userName = AnsiConsole.Ask<string>("Username: ");
            var password = AnsiConsole.Ask<string>("Password: ");
            EmployeeManager manager = new EmployeeManager(new HttpClient { BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiEmployeesUrl")) });
            manager.LoginEmployeeAsync(userName, password);
        }

    }
}
