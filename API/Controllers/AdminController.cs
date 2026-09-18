using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Appointments()
        {
            return View();
        }
    }
}