﻿using AI_Wardrobe.Models;
using SendGrid;
using SendGrid.Helpers.Mail;    

namespace AI_Wardrobe.Data.Services
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<Response> SendSingleEmail(ComposeEmailModel payload)
        {
            var apiKey = _configuration.GetSection("sendgridApiKey").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("aiwardrobe4@gmail.com",
                                        "AI Wardrobe");
            var to = new EmailAddress(payload.Email);

            var msg = MailHelper.CreateSingleEmail(from, to, payload.Subject,
                                                   payload.Body, payload.Body);

            return await client.SendEmailAsync(msg);
        }
    }
}
