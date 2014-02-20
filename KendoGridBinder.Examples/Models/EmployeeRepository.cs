using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace KendoGridBinder.Examples.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly KendoDataContext _db = new KendoDataContext();

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees;
        }

        public IEnumerable<Employee> GetPaged(int skip, int take, string sort, string filter)
        {
            // using pattern to demonstrate scenario where IQueryable is evaluated in a different context / scope
            using(var db = new KendoDataContext()){
                return db.Employees.Where(filter).OrderBy(sort).Skip(skip).Take(take).ToList();
            }
        }

        public int GetCount(string filter)
        {
            using (var db = new KendoDataContext())
            {
                return db.Employees.Where(filter).Count();
            }
        }
    }

    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> GetPaged(int skip, int take, string sort, string filter);
        int GetCount(string filter);
    }
}