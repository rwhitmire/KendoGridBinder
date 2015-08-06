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
            ViewBag.Callback = "Grid";
            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.Callback = "Grid2";
            return View("Index");
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

        [HttpPost]
        public JsonResult Grid2(KendoGridRequest request)
        {
            var sort = request.GetSorting();
            var filter = request.GetFiltering<Employee>();

            var employees = _employeeRepository.GetPaged(request.Skip, request.Take, sort, filter);
            var count = _employeeRepository.GetCount(filter);

            return Json(new KendoGrid<Employee>(employees, count));
        }
		public ActionResult AdvancedFiltering() {
			ViewBag.Callback = "Grid";
			return View();
		}
	}
}