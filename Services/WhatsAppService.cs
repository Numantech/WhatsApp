using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhatsAppAPI.Models;
using System.Text.RegularExpressions;



namespace WhatsAppAPI.Services
{
    public class WhatsAppService
    {

        private readonly HttpClient _httpClient;

        public WhatsAppService(string baseUrl, string token)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task SendMessage(string to, string message)
        {
            // Validate and format phone number
            string formattedNumber = FormatPhoneNumber(to);

            if (string.IsNullOrEmpty(formattedNumber))
            {
                throw new ArgumentException("Invalid phone number format.");
            }

            var payload = new
            {
                messaging_product = "whatsapp",
                to = formattedNumber,
                text = new { body = message }
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/v1/messages", content);
            response.EnsureSuccessStatusCode();
        }

        private string FormatPhoneNumber(string number)
        {
            var regex = new Regex(@"^\+?[1-9]\d{1,14}$"); // Matches + country code number
            number = number.Trim();
            if (regex.IsMatch(number))
            {
                return number;
            }

            //return null or handle the error
            return null;
        }

    }
}
