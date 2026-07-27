using BackendTest.Models;

namespace BackendTest.Interfaces
{
    public interface IEmployeeService
    {
        List<Employees> GetAll();

        Employees GetById(int id);

        void Add(Employees emp);

        void Update(Employees emp);

        void Delete(int id);
    }
}