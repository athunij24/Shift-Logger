using ShiftsApi.Models;
using Spectre.Console;

namespace ShiftLoggerApp
{
    class Program
    {
        public static string promptEmployee(string employeeName)
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Hello {employeeName}, what would you like to do?")
                .PageSize(10)
                .AddChoices(new[] {
                    "Log Shift", "View Shift(s)", "Update Shift", "Delete Shift",
                    "Quit"
                }));
            return choice;
        }

        public static string loginPrompt()
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .PageSize(10)
                .AddChoices(new[] {
                    "Login", "Register"
                }));
            return choice;
        }
        static async Task Main(string[] args)
        {
            string choice = loginPrompt();
            var httpClient = new HttpClient();
            var employeeManager = new EmployeeManager(httpClient);
            bool loggedIn = false;
            Employee currEmployee = null;

            while (!loggedIn)
            {
                if (choice == "Register")
                {
                    string name = AnsiConsole.Ask<string>("Name: ");
                    string userName = AnsiConsole.Ask<string>("Username: ");
                    string password = AnsiConsole.Ask<string>("Password: ");
                    string role = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What is your role?")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Employee", "Manager"
                        }));
                    Employee e = new Employee { Name = name, UserName = userName, Password = password, Role = role };
                    await employeeManager.RegisterEmployeeAsync(e);
                    Console.WriteLine("Registration in progress...");
                    await Task.Delay(5000);
                    AnsiConsole.Clear();
                    choice = loginPrompt();

                }
                else
                {
                    string loginUser = AnsiConsole.Ask<string>("Username: ");
                    string loginPass = AnsiConsole.Ask<string>("Password: ");
                    currEmployee = await employeeManager.LoginEmployeeAsync(loginUser, loginPass);
                    if (currEmployee != null)
                    {
                        loggedIn = true;
                    }
                }
            }
            AnsiConsole.WriteLine("Logging in...");
            await Task.Delay(2000);
            AnsiConsole.Clear();
            string action = promptEmployee(currEmployee.Name);
            while (action != "Quit")
            {
                switch (action)
                {
                    case "Log Shift":
                        await employeeManager.LogShiftAsync(currEmployee);
                        break;
                    case "View Shift(s)":
                        await employeeManager.ViewShiftsAsync(currEmployee);
                        await Task.Delay(10000);
                        break;
                    case "Update Shift":
                        await employeeManager.UpdateShiftAsync(currEmployee);
                        break;
                    case "Delete Shift":
                        await employeeManager.DeleteShiftAsync(currEmployee);
                        break;
                    case "Quit":
                        break;
                }
                AnsiConsole.Clear();
                action = promptEmployee(currEmployee.Name);
            }
        }
    }
}
