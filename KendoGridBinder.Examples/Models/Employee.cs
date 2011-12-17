using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace KendoGridBinder.Examples.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public int OfficeId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [ScriptIgnore]
        public virtual Office Office { get; set; }
    }
}