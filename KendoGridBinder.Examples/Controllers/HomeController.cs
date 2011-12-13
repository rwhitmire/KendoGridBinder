using System.Web.Mvc;
using KendoGridBinder.Examples.Models;

namespace KendoGridBinder.Examples.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repository = new Repository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Grid(KendoGridRequest request)
        {
            var data = _repository.GetGrid(request);
            return Json(data);
        }
    }
}
