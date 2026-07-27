using BackendTest.Models;// Importing the Models folder where the Employees class is defined
using Microsoft.EntityFrameworkCore;// Importing Entity Framework Core for database operations

namespace BackendTest.Context
{
    // This class represents the database context (bridge between C# and SQL Server)
    public class EmployeeDBContext : DbContext
    {
        // Constructor that receives configuration options from Program.cs (Dependency Injection)
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options)
        {
        }
        // This represents the Employees table in the database
        // DbSet allows you to perform CRUD operations (Create, Read, Update, Delete)
        public DbSet<Employees> Employees { get; set; }
    }
}
