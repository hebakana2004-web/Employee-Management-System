using BackendTest.Context;
using BackendTest.Interfaces;
using BackendTest.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendTest.Services
{
    public class LoginService : ILoginService
    {
        private readonly UsersDBContext _context;
        private readonly ILogger<LoginService> _log;

        public LoginService(ILogger<LoginService> logger, UsersDBContext context)
        {
            _log = logger;
            _context = context;
        }

        // GENERATE TOKEN
        public string GenerateToken(Users model)
        {
            try
            {
                _log.LogInformation("Generating token for user: {UserName}", model.UserName);

                if (model == null)
                    throw new Exception("Login data is null");

                // Get user from DATABASE
                var user = _context.Users
                    .FirstOrDefault(x => x.UserName == model.UserName);

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    throw new Exception("Invalid username or password");

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("MySuperSecretKeyForJwtToken12345"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error generating token");
                throw;
            }
        }

        // GET ALL USERS
        public List<Users> GetAllUsers()
        {
            try
            {
                _log.LogInformation("Fetching all users");

                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching users");
                throw;
            }
        }

        // GET USER BY ID
        public Users GetUserById(int id)
        {
            try
            {
                return _context.Users.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching user by id");
                throw;
            }
        }

        // ADD USER
        public void AddUser(Users model)
        {
            try
            {
                if (model == null)
                    throw new Exception("User is null");

                // Hash password before saving
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                _context.Users.Add(model);

                // SAVE TO DATABASE
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error adding user");
                throw;
            }
        }

        // UPDATE USER
        public void UpdateUser(int id, Users model)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == id);

                if (user == null)
                    throw new Exception("User not found");

                user.UserName = model.UserName;
                user.Role = model.Role;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                }

                // SAVE CHANGES
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error updating user");
                throw;
            }
        }

        // DELETE USER
        public void DeleteUser(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == id);

                if (user == null)
                    throw new Exception("User not found");

                _context.Users.Remove(user);

                // SAVE CHANGES
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error deleting user");
                throw;
            }
        }
    }
}