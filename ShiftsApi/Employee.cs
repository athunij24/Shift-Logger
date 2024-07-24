namespace ShiftsApi.Models
{
    public class Employee
    {
        public long Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public string Role { get; set; }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
