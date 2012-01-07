using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
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
            Mapper.CreateMap<Employee, EmployeeDto>();
            var employees = _employeeRepository.All;
            var dto = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            var grid = new KendoGrid<EmployeeDto>(request, dto);
            return Json(grid);
        }
    }
}