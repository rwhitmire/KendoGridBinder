using System.Collections.Generic;
using System.Linq;

namespace KendoGridBinder.Examples.Models
{
    public class Repository
    {
        private readonly KendoDataContext _db = new KendoDataContext();

        public IEnumerable<Employee> GetEmployees()
        {
            var employees = _db.Employees;
            return employees.ToList();
        }
    }
}