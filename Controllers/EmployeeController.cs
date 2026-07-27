using BackendTest.Interfaces;
using BackendTest.Models;
using BackendTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Controllers
{
    [Route("api/[controller]")]// Base route: api/Employee
    [ApiController]
    public class EmployeeController : ControllerBase// This class handles HTTP requests (GET, POST, PUT, DELETE)
    {
        private readonly IEmployeeService _service;// Service layer object used to access business logic (CRUD operations)
        private ILogger<EmployeeController> _log;// Logger used to track info, warnings, and errors
        public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)// Constructor (Dependency Injection)
        {
            _service = service; // Inject employee service
            _log = logger;// Inject logger
        }

        // GET all employees
      //  [Authorize]
        [HttpGet("GetEmployees")]// Endpoint: GET api/Employee/GetEmployees
        public IActionResult GetEmployees()// Method that returns all employees
        {
            try// Start error handling block
            {
                var employees = _service.GetAll();// Get all employees from service layer

                if (employees == null || !employees.Any())// Check if list is empty or null
                {
                    _log.LogWarning("No employees found");// Log warning if empty
                    return NotFound("No employees available");// Return 404 if no data exists
                }

                _log.LogInformation("Employees retrieved successfully");// Log success message

                return Ok(employees);// Return 200 OK with employees data
            }
            catch (Exception ex)// Catch any unexpected errors
            {
                _log.LogError(ex, "Error while getting employees");// Log error details
                return StatusCode(500, "Internal server error");// Return 500 server error
            }
        }
        // GET employee by id
       // [Authorize]
        [HttpGet("GetEmployeeById/{id}")]// GET api/Employee/GetEmployeeById/1
        public IActionResult GetEmployeeById(int id)// Method to get one employee by ID
        {
            try
            {
                if (id <= 0)// Validate ID (must be positive number)
                {
                    _log.LogWarning("Invalid ID received: {Id}", id);// Log invalid input
                    return BadRequest("Invalid employee ID");// Return 400 bad request
                }

                var employee = _service.GetById(id); // Search employee in service layer by id

                if (employee == null)// If employee not found
                {
                    _log.LogWarning("Employee not found with ID: {Id}", id);// Log warning
                    return NotFound("Employee not found");// Return 404 not found
                }

                _log.LogInformation("Employee retrieved successfully with ID: {Id}", id);// Log success

                return Ok(employee);// Return 200 OK with employee data
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while getting employee with ID: {Id}", id);// Log exception
                return StatusCode(500, "Internal server error");// Return 500 error
            }
        }

        // POST employee (Add new employee)
      //  [Authorize]
        [HttpPost("AddEmployee")]// POST api/Employee/AddEmployee
        public IActionResult AddEmployee(Employees emp)// Method to add new employee
        {
            try
            {
                if (emp == null)// Check if request body is empty
                {
                    _log.LogWarning("Received null employee object");// Log warning
                    return BadRequest("Employee data is required");// Return 400 bad request
                }

                _service.Add(emp);// Add employee to service (save data)

                _log.LogInformation("Employee added successfully: {Name}", emp.Name); // Log success

                return CreatedAtAction(  // Return 201 Created (best practice for POST)
                    nameof(GetEmployeeById),// Link to GET method
                    new { id = emp.Id },// Route parameter for new employee
                    emp// Return created employee data
                );
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error while adding employee");// Log error
                return StatusCode(500, "Internal server error");// Return server error
            }
        }
        // PUT employee
      //  [Authorize]
        [HttpPut("UpdateEmployee")]// Endpoint: PUT api/Employee/UpdateEmployee
        public IActionResult UpdateEmployee(Employees emp)// Method to update employee
        {
            try
            {
                if (emp == null || emp.Id <= 0)// Validate input data
                {
                    _log.LogWarning("Invalid employee data received for update");// Log warning
                    return BadRequest("Invalid employee data");// Return 400
                }

                var existingEmployee = _service.GetById(emp.Id); // Check if employee exists

                if (existingEmployee == null)// If not found
                {
                    _log.LogWarning("Employee not found for update: {Id}", emp.Id);// Log warning
                    return NotFound("Employee not found");// Return 404
                }

                _service.Update(emp);// Update employee data

                _log.LogInformation("Employee updated successfully: {Id}", emp.Id);// Log success

                return Ok(emp); // Return updated employee
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error while updating employee with ID: {Id}", emp?.Id);// Log error
                return StatusCode(500, "Internal server error");// Return 500 error
            }
        }

        // DELETE employee
     //   [Authorize]
        [HttpDelete("DeleteEmployee/{id}")]// Endpoint: DELETE api/Employee/DeleteEmployee/1
        public IActionResult DeleteEmployee(int id)// Method to delete employee
        {
            try
            {
                if (id <= 0)// Validate ID
                {
                    _log.LogWarning("Invalid delete request, ID: {Id}", id);// Log warning
                    return BadRequest("Invalid employee ID"); // Return 400
                }

                var employee = _service.GetById(id);// Check if employee exists

                if (employee == null)// If not found
                {
                    _log.LogWarning("Employee not found for deletion: {Id}", id);// Log warning
                    return NotFound("Employee not found");// Return 404
                }

                _service.Delete(id); // Delete employee

                _log.LogInformation("Employee deleted successfully: {Id}", id);// Log success

                return NoContent(); // Return 204 (success but no content)
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error while deleting employee with ID: {Id}", id);// Log error
                return StatusCode(500, "Internal server error");// Return 500
            }
        }
    }
}