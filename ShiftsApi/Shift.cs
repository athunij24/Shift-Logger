﻿namespace ShiftsApi.Models
{
    public class Shift
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public long EmployeeId { get; set; }
    }
}

