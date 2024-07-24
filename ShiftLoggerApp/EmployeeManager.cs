using Newtonsoft.Json;
using ShiftsApi.Models;
using Spectre.Console;
using System.Text;


namespace ShiftLoggerApp
{
    public class EmployeeManager
    {
        private readonly HttpClient _httpClient;
        public EmployeeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> RegisterEmployeeAsync(Employee employee)
        {
            var url = Environment.GetEnvironmentVariable("ApiEmployeesUrl");
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    AnsiConsole.WriteLine("Successfully registered");
                    return true;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    Console.WriteLine("Conflict: " + await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine("Error: " + await response.Content.ReadAsStringAsync());
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
            var url = $"{Environment.GetEnvironmentVariable("ApiEmployeesUrl")}login";
            try
            {
                var loginRequest = new { Username = userName, Password = password };
                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Employee>(jsonResponse);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Invalid Login: " + await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine("Error: " + await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Request error: " + e.Message);
                return null;
            }
        }



        public async Task LogShiftAsync(Employee currEmployee)
        {
            AnsiConsole.WriteLine("Please enter date and time in format 08/18/2018 12:07 PM");
            string startTime = AnsiConsole.Ask<string>("Start: ");
            string endTime = AnsiConsole.Ask<string>("End: ");
            Shift shift = new Shift
            {
                StartTime = DateTime.Parse(startTime),
                EndTime = DateTime.Parse(endTime),
                EmployeeId = currEmployee.Id
            };
            var url = Environment.GetEnvironmentVariable("ApiShiftsUrl");
            var json = JsonConvert.SerializeObject(shift);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    AnsiConsole.WriteLine("Logged shift successfully");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    AnsiConsole.WriteLine("Conflict: " + await response.Content.ReadAsStringAsync());
                }
                else
                {
                    AnsiConsole.WriteLine("Error: " + await response.Content.ReadAsStringAsync());
                }

            }
            catch (HttpRequestException e)
            {
                AnsiConsole.WriteLine("Request error: " + e.Message);
            }
        }

        public async Task ViewShiftsAsync(Employee currEmployee)
        {
            var url = Environment.GetEnvironmentVariable("ApiShiftsUrl");
            List<Shift> shifts;

            try
            {
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                shifts = JsonConvert.DeserializeObject<List<Shift>>(responseContent);
            }
            catch (HttpRequestException e)
            {
                AnsiConsole.WriteLine("Request error: " + e.Message);
                return;
            }

            List<Shift> filteredList;

            if (currEmployee.Role == "Employee")
            {
                shifts = shifts.Where(s => s.EmployeeId == currEmployee.Id).ToList();
            }

            if (shifts == null || shifts.Count == 0)
            {
                AnsiConsole.WriteLine("No shifts have been logged");
                return;
            }

            var table = new Table();
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Length");
            table.AddColumn("Employee Id");
            table.AddColumn("Shift id");
            foreach (Shift s in shifts)
            {
                table.AddRow(s.StartTime.ToString(), s.EndTime.ToString(), (s.EndTime - s.StartTime).ToString(), s.EmployeeId.ToString(), s.Id.ToString());
            }
            AnsiConsole.Write(table);
        }

        public async Task UpdateShiftAsync(Employee currEmployee)
        {
            if (currEmployee.Role == "Employee")
            {
                AnsiConsole.WriteLine("You need to be a manager to do this");
                await Task.Delay(5000);
                return;
            }
            await ViewShiftsAsync(currEmployee);
            long shiftId = AnsiConsole.Ask<long>("What is the id of the shift you want to update?");
            AnsiConsole.WriteLine("Please enter new date and time in format 08/18/2018 12:07 PM");
            string startTime = AnsiConsole.Ask<string>("New Start: ");
            string endTime = AnsiConsole.Ask<string>("New End: ");
            long employeeId = AnsiConsole.Ask<long>("New employee Id: ");
            Shift newShift = new Shift
            {
                StartTime = DateTime.Parse(startTime),
                EmployeeId = employeeId,
                EndTime = DateTime.Parse(endTime),
                Id = shiftId
            };
            var url = $"{Environment.GetEnvironmentVariable("ApiShiftsUrl")}{shiftId}";
            var jsonContent = JsonConvert.SerializeObject(newShift);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PutAsync(url, content);
                AnsiConsole.WriteLine("Update was a success");
            }
            catch (HttpRequestException ex)
            {
                AnsiConsole.WriteLine(ex.Message);
            }


        }

        public async Task DeleteShiftAsync(Employee currEmployee)
        {
            await ViewShiftsAsync(currEmployee);
            long shiftId = AnsiConsole.Ask<long>("What is the id of the shift you want to delete?");
            var url = $"{Environment.GetEnvironmentVariable("ApiShiftsUrl")}{shiftId}";
            try
            {
                var response = await _httpClient.DeleteAsync(url);
            }
            catch (HttpRequestException ex)
            {
                AnsiConsole.WriteLine(ex.Message);
            }
        }
    }
}
