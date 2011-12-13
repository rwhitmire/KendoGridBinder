namespace KendoGridBinder.Examples.Models
{
    public class Repository
    {
        private readonly KendoDataContext _db = new KendoDataContext();

        public KendoGrid<Employee> GetGrid(KendoGridRequest request)
        {
            var query = _db.Employees;
            return new KendoGrid<Employee>(request, query);
        }
    }
}