using System;

namespace KendoGridBinder.Examples.Models
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public bool Active { get; set; }
    }
}