using Azure.Messaging;
using BackendTest.Interfaces;
using BackendTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;
        private readonly ILogger<LoginController> _log;

        public LoginController(ILoginService service, ILogger<LoginController> logger)
        {
            _service = service;
            _log = logger;
        }

        // LOGIN (Generate JWT Token)
        [HttpPost("login")]
        public ActionResult<object> Login(Users model)
        {
            try
            {
                _log.LogInformation("Login request received for user: {UserName}", model.UserName);

                var token = _service.GenerateToken(model);

                return Ok(new
                {
                    token = token
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Login failed for user: {UserName}", model?.UserName);

                return BadRequest(new
                {
                    MessageContent = ex.Message
                });
            }
        }

        // GET ALL USERS
        [HttpGet("GetUsers")]
        public ActionResult<List<Users>> GetUsers()
        {
            try
            {
                var users = _service.GetAllUsers();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching users");

                return BadRequest(new
                {
                    MessageContent = ex.Message
                });
            }
        }

        // GET USER BY ID
        [HttpGet("GetUserById/{id}")]
        public ActionResult<Users> GetUserById(int id)
        {
            try
            {
                var user = _service.GetUserById(id);

                if (user == null)
                {
                    return NotFound( new {
                        MessageContent = "User not found"
                });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching user by ID: {Id}", id);

                return BadRequest(new
                {
                    MessageContent = ex.Message
                });
            }
        }

        // ADD USER
        [HttpPost("AddUser")]
        public ActionResult<string> AddUser(Users model)
        {
            try
            {
                _service.AddUser(model);

                return Ok(new { MessageContent= "User added successfully" });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error adding user");

                return BadRequest(new
                {
                    MessageContent = ex.Message
                });
            }
        }

        // UPDATE USER
        [HttpPut("UpdateUser/{id}")]
        public ActionResult<string> UpdateUser(int id, Users model)
        {
            try
            {
                _service.UpdateUser(id, model);

                return Ok(new {MessageContent= "User updated successfully"});
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error updating user with ID: {Id}", id);

                return BadRequest(new
                {
                    MessageContent = ex.Message
                });
            }
        }

        // DELETE USER
        [HttpDelete("DeleteUser/{id}")]
        public ActionResult<string> DeleteUser(int id)
        {
            try
            {
                _service.DeleteUser(id);

                return Ok(new {MessageContent= "User deleted successfully" });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error deleting user with ID: {Id}", id);

                return BadRequest(new
                {
                    MessageContent = ex.Message
                });
            }
        }
    }
}