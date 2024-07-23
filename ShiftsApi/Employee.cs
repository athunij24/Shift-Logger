using System;

namespace ShiftsApi.Models
{
	public class Employee
	{
		public long Id { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }

		public string Role { get; set; }

		public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
	}
}
