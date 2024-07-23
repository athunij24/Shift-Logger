using System;
using Microsoft.EntityFrameworkCore;

namespace ShiftsApi.Models
{
    public class ShiftDbContext : DbContext
    {
        public ShiftDbContext(DbContextOptions<ShiftDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shift> Shifts { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shift>().HasOne(e => e.Employee).WithMany(e => e.Shifts).HasForeignKey(e => e.EmployeeId);
        }
    }
}
