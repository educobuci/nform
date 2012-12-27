using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFormSample.Models;

namespace NFormSample.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Car car = new Car();
            return View(car);
        }

        public ActionResult Create(Models.Car car)
        {
            if (car.Validations.IsInvalid)
            {
                return Content(car.Validations.Errors.Aggregate("", (b, m) => String.Format("{0}<li>{1} {2}</li>", b, m.Key, m.Value)));
            }
            return Content("success");
        }
    }
}