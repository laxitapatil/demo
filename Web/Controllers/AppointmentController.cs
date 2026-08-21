using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AppointmentController : Controller
    {
        [HttpGet]
        public IActionResult Book()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Book(string name, string mobile, string address, string pincode, string reason)
        {
            // TODO: later, save this to the database or call the Api project's endpoint
            ViewBag.Success = true;
            return View();
        }
    }
}