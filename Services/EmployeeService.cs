using BackendTest.Context;
using BackendTest.Interfaces;
using BackendTest.Models;
using System;

namespace BackendTest.Services //// Defines logical grouping for service layer
{
    public class EmployeeService : IEmployeeService // Implements interface (must follow contract rules)
    {
        private readonly EmployeeDBContext _context;

        private ILogger<EmployeeService> _log;// Logger used to record info, warnings, and errors for debugging and monitoring

        public EmployeeService(ILogger<EmployeeService> logger, EmployeeDBContext context) //inject in constructor{ Constructor (Dependency Injection)}
        {
            _log = logger; // initiate
            _context = context;
        }

        public List<Employees> GetAll()// Method to return all employees
        {
            try// Start error handling block
            {
                _log.LogInformation("Fetching all employees started"); // better than Debug for tracing flow,Log info that method execution has started

                var employeesList = _context.Employees.ToList() ?? new List<Employees>(); // null safety,  If employees is null, create empty list (null safety)


                if (!employeesList.Any())// Check if list is empty (no employees exist)
                {
                    _log.LogWarning("No employees found in the system"); // clear warning message
                    return new List<Employees>(); // return empty list safely
                }

                _log.LogInformation("Successfully retrieved {Count} employees", employeesList.Count); // useful structured log

                return employeesList;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unexpected error occurred while fetching employees"); // correct logging format

                throw; // preserves original stack trace (IMPORTANT)
            }
        }

        public Employees GetById(int id)
        {
            try
            {
                _log.LogInformation("Fetching employee with ID: {Id}", id); // structured logging

                var employee = _context.Employees.FirstOrDefault(e => e.Id == id); // search employee

                if (employee == null)
                {
                    _log.LogWarning("Employee not found with ID: {Id}", id); // better tracking
                    return null; // explicitly return null if not found
                }

                _log.LogInformation("Employee found successfully with ID: {Id}", id);

                return employee;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unexpected error occurred while fetching employee with ID: {Id}", id); // correct format

                throw; // preserves original stack trace
            }
        }

        public void Add(Employees emp)
        {
            try
            {
                if (emp == null)
                {
                    _log.LogWarning("Attempted to add null employee");
                    throw new ArgumentNullException(nameof(emp));
                }

                var exists = _context.Employees.Any(e => e.Id == emp.Id);

                if (exists)
                {
                    _log.LogWarning("Employee already exists with ID: {Id}", emp.Id);
                    throw new InvalidOperationException($"Employee with ID {emp.Id} already exists");
                }

                _context.Employees.Add(emp);
                _context.SaveChanges();


                _log.LogInformation("Employee added successfully: {Id}, {Name}", emp.Id, emp.Name);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while adding employee with ID: {Id}", emp?.Id);
                throw;
            }
        }
        public void Update(Employees emp)
        {
            try
            {
                if (emp == null)
                {
                    _log.LogWarning("Attempted to update with null employee");
                    throw new ArgumentNullException(nameof(emp));
                }

                var existing = _context.Employees.FirstOrDefault(e => e.Id == emp.Id);

                if (existing == null)
                {
                    _log.LogWarning("Employee not found for update: {Id}", emp.Id);
                    throw new KeyNotFoundException($"Employee with ID {emp.Id} not found");
                }

                // Update fields
                existing.Name = emp.Name;

                // If you have more fields, update them here:
                // existing.Age = emp.Age;
                // existing.Department = emp.Department;
                _context.SaveChanges();


                _log.LogInformation("Employee updated successfully: {Id}", emp.Id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while updating employee with ID: {Id}", emp?.Id);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _log.LogWarning("Invalid delete request, ID: {Id}", id);
                    throw new ArgumentException("Invalid employee ID");
                }

                var emp = _context.Employees.FirstOrDefault(e => e.Id == id);

                if (emp == null)
                {
                    _log.LogWarning("Employee not found for deletion: {Id}", id);
                    throw new KeyNotFoundException($"Employee with ID {id} not found");
                }

                _context.Employees.Remove(emp);
                _context.SaveChanges(); 

                _log.LogInformation("Employee deleted successfully: {Id}", id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while deleting employee with ID: {Id}", id);
                throw;
            }
        }
    }
}