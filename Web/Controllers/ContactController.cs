using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string fullName, string email, string phone, string message)
        {
            // TODO: save to database or send email later
            ViewBag.Success = true;
            return View();
        }
    }
}