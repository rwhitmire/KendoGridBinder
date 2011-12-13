using System.Data.Entity;

namespace KendoGridBinder.Examples.Models
{
    public class KendoDataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}