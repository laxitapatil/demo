using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web.Services
{
    public class AppointmentApiService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private static string? _cachedToken;
        private static DateTime _tokenExpiry = DateTime.MinValue;

        public AppointmentApiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        private async Task<string?> GetTokenAsync()
        {
            if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry)
                return _cachedToken;

            var baseUrl = _config["ApiSettings:BaseUrl"];
            var username = _config["ApiSettings:UserName"];
            var password = _config["ApiSettings:Password"];

            var loginBody = new
            {
                UserName = username,
                Password = password,
                Device_type = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(loginBody), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync($"{baseUrl}/api/Authenticate/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            _cachedToken = doc.RootElement.GetProperty("token").GetString();
            var expiry = doc.RootElement.GetProperty("expiry").GetDateTime();
            _tokenExpiry = expiry.AddMinutes(-1);

            return _cachedToken;
        }

        public async Task<(bool Success, string Message)> CreateAppointmentAsync(
            string candidateName, string mobile, string email, DateTime startTime, DateTime endTime, string note)
        {
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var companyId = _config["ApiSettings:CompanyId"];

            var token = await GetTokenAsync();
            if (token == null)
                return (false, "Unable to connect to booking system. Please try again later.");

            var body = new
            {
                id = 0,
                company_id = companyId,
                candidate_name = candidateName,
                mobile_num = mobile,
                email = email,
                startTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                endTime = endTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                note = note
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/appointment")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return (true, "Appointment booked successfully.");

            try
            {
                using var doc = JsonDocument.Parse(responseText);
                if (doc.RootElement.TryGetProperty("detail", out var detail))
                    return (false, detail.GetString() ?? "Booking failed. Please try again.");
            }
            catch { }

            return (false, "Booking failed. Please try again.");
        }
    }
}