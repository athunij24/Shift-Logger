using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using ShiftsApi.Models;
using Newtonsoft.Json;


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
            var url = $"{Environment.GetEnvironmentVariable("ApiEmployeesUrl")}register";
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
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

                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Request error: " + e.Message);
                return false;
            }
        }

        public async Task<Employee> LoginEmployeeAsync(string userName, string passWord)
        {
            var url = $"{Environment.GetEnvironmentVariable("ApiEmployeesUrl")}login";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Employee>(jsonResponse);
                }
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
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

        //public async Task<List<Shift>> GetShiftsForEmployeeAsync(Employee employee)
        //{

        //}
    }
}
