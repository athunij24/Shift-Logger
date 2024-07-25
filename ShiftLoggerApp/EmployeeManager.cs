using Newtonsoft.Json;
using ShiftsApi.Models;
using Spectre.Console;
using System.Text;

namespace ShiftLoggerApp
{
    public class EmployeeManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEmployeesUrl;
        private readonly string _apiShiftsUrl;

        public EmployeeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiEmployeesUrl = Environment.GetEnvironmentVariable("ApiEmployeesUrl") ?? throw new InvalidOperationException("API Employees URL not set.");
            _apiShiftsUrl = Environment.GetEnvironmentVariable("ApiShiftsUrl") ?? throw new InvalidOperationException("API Shifts URL not set.");
        }

        private StringContent CreateJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<string> HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return content;
            }

            Console.WriteLine($"Error {response.StatusCode}: {content}");
            return null;
        }

        public async Task<bool> RegisterEmployeeAsync(Employee employee)
        {
            var url = $"{_apiEmployeesUrl}register";
            var content = CreateJsonContent(employee);

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var result = await HandleResponse(response);
                if (result != null)
                {
                    AnsiConsole.WriteLine("Successfully registered");
                    return true;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Request error: " + e.Message);
            }

            return false;
        }

        public async Task<Employee> LoginEmployeeAsync(string userName, string password)
        {
            var url = $"{_apiEmployeesUrl}login";
            var loginRequest = new { Username = userName, Password = password };
            var content = CreateJsonContent(loginRequest);

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var result = await HandleResponse(response);
                return result != null ? JsonConvert.DeserializeObject<Employee>(result) : null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Request error: " + e.Message);
                return null;
            }
        }

        public async Task LogShiftAsync(Employee currEmployee)
        {
            DateTime start, end;
            while (true)
            {
                try
                {
                    start = DateTime.Parse(AnsiConsole.Ask<string>("Start: "));
                    end = DateTime.Parse(AnsiConsole.Ask<string>("End: "));
                    break;
                }
                catch (FormatException)
                {
                    AnsiConsole.WriteLine("Please enter date and time in correct format.");
                }
            }

            if ((end - start).TotalMinutes < 0)
            {
                AnsiConsole.WriteLine("Shift must end before starting, returning to menu.");
                await Task.Delay(3000);
                return;
            }
            var shift = new Shift
            {
                StartTime = start,
                EndTime = end,
                EmployeeId = currEmployee.Id
            };

            var url = _apiShiftsUrl;
            var content = CreateJsonContent(shift);

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var result = await HandleResponse(response);
                if (result != null)
                {
                    AnsiConsole.WriteLine("Logged shift successfully");
                    await Task.Delay(3000);
                }
            }
            catch (HttpRequestException e)
            {
                AnsiConsole.WriteLine("Request error: " + e.Message);
            }
        }

        public async Task<List<Shift>> ViewShiftsAsync(Employee currEmployee)
        {
            var url = _apiShiftsUrl;
            List<Shift> shifts = null;

            try
            {
                var response = await _httpClient.GetAsync(url);
                var responseContent = await HandleResponse(response);
                shifts = responseContent != null ? JsonConvert.DeserializeObject<List<Shift>>(responseContent) : new List<Shift>();
            }
            catch (HttpRequestException e)
            {
                AnsiConsole.WriteLine("Request error: " + e.Message);
                return null;
            }

            if (currEmployee.Role == "Employee")
            {
                shifts = shifts.Where(s => s.EmployeeId == currEmployee.Id).ToList();
            }

            if (!shifts.Any())
            {
                AnsiConsole.WriteLine("No shifts have been logged");
                return null;
            }

            DisplayShiftsTable(shifts);
            return shifts;
        }

        private void DisplayShiftsTable(List<Shift> shifts)
        {
            var table = new Table();
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Length");
            table.AddColumn("Employee Id");
            table.AddColumn("Shift Id");

            foreach (var shift in shifts)
            {
                table.AddRow(
                    shift.StartTime.ToString(),
                    shift.EndTime.ToString(),
                    (shift.EndTime - shift.StartTime).ToString(),
                    shift.EmployeeId.ToString(),
                    shift.Id.ToString());
            }

            AnsiConsole.Write(table);
        }

        public async Task UpdateShiftAsync(Employee currEmployee)
        {
            if (currEmployee.Role != "Manager")
            {
                AnsiConsole.WriteLine("You need to be a manager to do this");
                await Task.Delay(5000);
                return;
            }

            var shifts = await ViewShiftsAsync(currEmployee);
            if (shifts == null || !shifts.Any())
            {
                return;
            }

            var shiftId = AnsiConsole.Ask<long>("What is the id of the shift you want to update?");
            if (!shifts.Any(s => s.Id == shiftId))
            {
                AnsiConsole.WriteLine("Invalid shift id, returning to menu...");
                await Task.Delay(5000);
                return;
            }

            DateTime start, end;
            while (true)
            {
                try
                {
                    start = DateTime.Parse(AnsiConsole.Ask<string>("New Start: "));
                    end = DateTime.Parse(AnsiConsole.Ask<string>("New End: "));
                    break;
                }
                catch (FormatException)
                {
                    AnsiConsole.WriteLine("Please enter date and time in correct format.");
                }
            }

            var employeeId = AnsiConsole.Ask<long>("New Employee Id");
            var newShift = new Shift
            {
                StartTime = start,
                EndTime = end,
                EmployeeId = employeeId,
                Id = shiftId
            };

            var url = $"{_apiShiftsUrl}{shiftId}";
            var content = CreateJsonContent(newShift);

            try
            {
                var response = await _httpClient.PutAsync(url, content);
                var result = await HandleResponse(response);
                if (result != null)
                {
                    AnsiConsole.WriteLine("Update was successful");
                }
            }
            catch (HttpRequestException ex)
            {
                AnsiConsole.WriteLine("Request error: " + ex.Message);
            }
        }

        public async Task DeleteShiftAsync(Employee currEmployee)
        {
            var shifts = await ViewShiftsAsync(currEmployee);
            if (shifts == null || !shifts.Any())
            {
                return;
            }

            var shiftId = AnsiConsole.Ask<long>("What is the id of the shift you want to delete?");
            if (!shifts.Any(s => s.Id == shiftId))
            {
                AnsiConsole.WriteLine("Invalid shift id, returning to menu...");
                await Task.Delay(5000);
                return;
            }

            var url = $"{_apiShiftsUrl}{shiftId}";
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                var result = await HandleResponse(response);
                if (result != null)
                {
                    AnsiConsole.WriteLine("Shift deleted successfully");
                }
            }
            catch (HttpRequestException ex)
            {
                AnsiConsole.WriteLine("Request error: " + ex.Message);
            }
        }
    }
}
