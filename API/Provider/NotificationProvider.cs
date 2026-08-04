using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;

namespace Api.Provider
{
    public class NotificationProvider : IDisposable
    {
        private readonly IConfiguration _configuration;

        public void Dispose() { }
        public NotificationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendWhatsAppMessage(string phoneNumber, string message)
        {
            try
            {
                string encodedMessage = Uri.EscapeDataString(message); 
                string url = string.Format(
                    _configuration.GetValue<string>("WhatsappMessage"),
                    phoneNumber,
                    encodedMessage);

                HttpResponseMessage response = await new HttpClient().GetAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return false;
            }
        }

        public async Task<bool> SendWhatsAppMessage(string message)
        {
            try
            {
                string adminNumber = _configuration.GetValue<string>("AdminWhatsAppNo");
                return await SendWhatsAppMessage(adminNumber, message);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}