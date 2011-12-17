using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KendoGridBinder.Examples.Models
{
    public class Office
    {
        private ICollection<Employee> _employees;

        public int OfficeId { get; set; }

        [Required]
        [StringLength(50)]
        public string OfficeName { get; set; }

        public virtual ICollection<Employee> Employees
        {
            get
            {
                return _employees ?? (_employees = new List<Employee>());
            }
            set
            {
                _employees = value;
            }
        }
    }
}