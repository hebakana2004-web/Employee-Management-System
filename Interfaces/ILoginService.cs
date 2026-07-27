using BackendTest.Models;

namespace BackendTest.Interfaces
{
    public interface ILoginService
    {
        string GenerateToken(Users model);

        List<Users> GetAllUsers();

        Users GetUserById(int id);

        void AddUser(Users model);

        void UpdateUser(int id, Users model);

        void DeleteUser(int id);
    }
}