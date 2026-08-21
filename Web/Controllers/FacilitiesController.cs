using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class FacilitiesController : Controller
    {
        public IActionResult LaserRefractive()
        {
            return View();
        }

        public IActionResult RefractiveErrors()
        {
            return View();
        }

        public IActionResult CataractSurgery()
        {
            return View();
        }

        public IActionResult GlaucomaSurgery()
        {
            return View();
        }

        public IActionResult OptiLasik()
        {
            return View();
        }
    }
}