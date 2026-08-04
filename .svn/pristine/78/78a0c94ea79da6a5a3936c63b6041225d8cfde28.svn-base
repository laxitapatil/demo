using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;

namespace Api.Provider
{
    public class WhatsAppProvider : IDisposable
    {
        private readonly IConfiguration _configuration;

        public void Dispose() {  }
        public WhatsAppProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Send(string phoneNumber, string message)
        {
            try
            {
                //if (phoneNumber.Length < 12)
                //{
                //    return false;
                //}
                string encodedMessage = Uri.EscapeDataString(message);
                string url = string.Format(
                    _configuration.GetValue<string>("WhatsappMessage"),
                    phoneNumber,
                    encodedMessage);
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await new HttpClient().GetAsync(url);
                    }
                    catch
                    { }
                });

                return true; 
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Send(string message)
        {
            try
            {
                string adminNumber = _configuration.GetValue<string>("AdminWhatsAppNo");
                return await Send(adminNumber, message);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}