using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TreatmentController : Controller
    {
        public IActionResult CataractSurgery() => View();
        public IActionResult Glaucoma() => View();
        public IActionResult OptiLasik() => View();
        public IActionResult BladelessLasik() => View();
        public IActionResult Perimetry() => View();
        public IActionResult FloatersAndFlashes() => View();
        public IActionResult YagLaser() => View();
        public IActionResult ContactLenses() => View();
    }
}