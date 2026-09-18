using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentApiService _apiService;

        public AppointmentController(AppointmentApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Book()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(string name, string mobile, string email, string address, string city, string pincode, string reason, DateTime preferredDate, string preferredTime)
        {
            string note = $"Reason: {reason} | Address: {address}, {city} - {pincode}";

            TimeSpan timeOfDay = TimeSpan.Parse(preferredTime);
            DateTime startTime = preferredDate.Date + timeOfDay;
            DateTime endTime = startTime.AddMinutes(30);

            var (success, message) = await _apiService.CreateAppointmentAsync(name, mobile, email, startTime, endTime, note);

            ViewBag.Success = success;
            ViewBag.Message = message;

            return View();
        }
    }
}