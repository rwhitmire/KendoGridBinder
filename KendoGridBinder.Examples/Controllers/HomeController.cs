﻿using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
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
            Mapper.CreateMap<Employee, EmployeeDto>();
            var employees = _repository.GetEmployees();
            var dto = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            var grid = new KendoGrid<EmployeeDto>(request, dto);
            return Json(grid);
        }
    }
}