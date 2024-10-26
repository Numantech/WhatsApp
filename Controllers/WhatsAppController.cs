using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsAppAPI.Data;
using WhatsAppAPI.Services;
using WhatsAppAPI.Models;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.EntityFrameworkCore;


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
            TwilioClient.Init("SID", "TOKEN");

        }


        [HttpPost("send")]
        public async Task<IActionResult> SendMessage( string userPhoneNumber)
        {
            var questions = new[]
    {
        "What is your name?",
        "What is your age?",
        "What is your city?",
        "Note: Please reply with all three answers in the format: Name, age , city"
    };

            var messageBody = string.Join("\n", questions);

            
                var message = MessageResource.Create(
                    body: messageBody,
                    from: new PhoneNumber("whatsapp:YOUR_NUMBER"),
                    to: new PhoneNumber($"whatsapp:{userPhoneNumber}")
                );
            

            return Ok("Messages sent!");
        }



        private async Task SendMessage(string userId, string thankssms)
        {
            var message = MessageResource.Create(
                body: thankssms,
                from: new PhoneNumber("whatsapp:+14155238886"), 
                    to: new PhoneNumber($"whatsapp:{userId}")
            );
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveMessage([FromForm] TwilioMessage message)
        {

            var userId = message.From; 

            
            var answers = message.Body.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (answers.Length >= 3)
            {
                var userResponse = new Response
                {
                    Question1 = answers[0].Trim(),
                    Question2 = answers[1].Trim(),
                    Question3 = answers[2].Trim(),
                    UserId = userId
                };
                  _context.Responses.Add(userResponse);
               
                await SendMessage(userId, "Thank you for your responses!");

                await _context.SaveChangesAsync();
            }
            else
            {
                await SendMessage(userId, "Please reply with all three answers in the format: Name, Age, City.");
            }

            return Ok();
        }





        //[HttpPost("send")]
        //public async Task<IActionResult> SendQuestions([FromBody] string userId, string userPhone)
        //{
        //    var questions = new List<string>
        //    {
        //        "Question 1: What is your Brand?",
        //        "Question 2: What is your hobby?",
        //        "Question 3: What is your dream job?"
        //    };

        //    foreach (var question in questions)
        //    {
        //        await _whatsAppService.SendMessage(userPhone, question);
        //    }

        //    return Ok("Questions sent.");
        //}

        //[HttpPost("response")]
        //public async Task<IActionResult> SaveResponse([FromBody] Response response)
        //{
        //    _context.Responses.Add(response);
        //    await _context.SaveChangesAsync();
        //    return Ok("Response saved.");
        //}
    }


    public class TwilioMessage
    {
        public string From { get; set; }
        public string Body { get; set; }
    }
}
