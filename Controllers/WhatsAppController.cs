using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsAppAPI.Data;
using WhatsAppAPI.Services;
using WhatsAppAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WhatsAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly WhatsAppContext _context;
        private readonly WhatsAppService _whatsAppService;

        public WhatsAppController(WhatsAppContext context, WhatsAppService whatsAppService)
        {
            _context = context;
            _whatsAppService = whatsAppService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendQuestions([FromBody] string userId, string userPhone)
        {
            var questions = new List<string>
            {
                "Question 1: What is your Brand?",
                "Question 2: What is your hobby?",
                "Question 3: What is your dream job?"
            };

            foreach (var question in questions)
            {
                await _whatsAppService.SendMessage(userPhone, question);
            }

            return Ok("Questions sent.");
        }

        [HttpPost("response")]
        public async Task<IActionResult> SaveResponse([FromBody] Response response)
        {
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            return Ok("Response saved.");
        }
    }
}
