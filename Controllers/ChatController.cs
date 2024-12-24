using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Chat_Bot_Mvc.Controllers
{
    public class ChatController : Controller
    {
        private readonly HttpClient _httpClient;

        public ChatController()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000") }; // Python API URL
        }
        public IActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetChatResponse(string userMessage)
        {
            var data = new { message = userMessage };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/chat", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var chatbotResponse = JsonConvert.DeserializeObject<ChatbotResponse>(result);
                return Json(chatbotResponse);
            }
            return Json(new { reply = "Failed to connect to the chatbot." });
        }

        public class ChatbotResponse
        {
            public string Reply { get; set; }
        }
    }
}
