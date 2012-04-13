using System.Collections.Generic;

namespace KendoGridBinder.Examples.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        readonly KendoDataContext _db = new KendoDataContext();

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees;
        } 
    }

    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
    }
}