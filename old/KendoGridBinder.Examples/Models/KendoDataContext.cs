using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KendoGridBinder.Examples.Models
{
    public class KendoDataContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<KendoGridBinder.Examples.Models.KendoDataContext>());

        public DbSet<Employee> Employees { get; set; }
    }

    public class KendoDataContextInitializer : DropCreateDatabaseIfModelChanges<KendoDataContext>
    {
        protected override void Seed(KendoDataContext context)
        {
            var employees = new List<Employee>
                                {
                                    new Employee{Id = 1, Active = true, Name = "Bill Smith", Email = "bsmith@email.com", EmployeeNumber = 1001, HireDate = Convert.ToDateTime("12/12/1997")}
                                };

            employees.ForEach(x => context.Employees.Add(x));
        }
    }
}