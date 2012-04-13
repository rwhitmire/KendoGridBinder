using System.Web.Mvc;
using KendoGridBinder.Examples.Models;

namespace KendoGridBinder.Examples.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController() : this(new EmployeeRepository())
        {
        }

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Grid(KendoGridRequest request)
        {
            var employees = _employeeRepository.GetAll();
            var data = new KendoGrid<Employee>(request, employees);
            return Json(data);
        }
    }
}