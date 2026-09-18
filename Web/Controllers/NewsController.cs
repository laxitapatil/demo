using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewBag.NewsId = id;
            return View();
        }
    }
}