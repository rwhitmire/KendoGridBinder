using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace KendoGridBinder.Examples.Models
{ 
    public class EmployeeRepository : IEmployeeRepository
    {
        KendoDataContext context = new KendoDataContext();

        public IQueryable<Employee> All
        {
            get { return context.Employees; }
        }

        public IQueryable<Employee> AllIncluding(params Expression<Func<Employee, object>>[] includeProperties)
        {
            IQueryable<Employee> query = context.Employees;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Employee Find(int id)
        {
            return context.Employees.Find(id);
        }

        public void InsertOrUpdate(Employee employee)
        {
            if (employee.EmployeeId == default(int)) {
                // New entity
                context.Employees.Add(employee);
            } else {
                // Existing entity
                context.Entry(employee).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var employee = context.Employees.Find(id);
            context.Employees.Remove(employee);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IEmployeeRepository
    {
        IQueryable<Employee> All { get; }
        IQueryable<Employee> AllIncluding(params Expression<Func<Employee, object>>[] includeProperties);
        Employee Find(int id);
        void InsertOrUpdate(Employee employee);
        void Delete(int id);
        void Save();
    }
}