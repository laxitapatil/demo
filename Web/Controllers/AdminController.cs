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

        public IActionResult PatientFeedback()
        {
            return View();
        }

        public IActionResult Subscribers()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}