using BetaCinema.Application.DTOs;
using BetaCinema.Application.Interfaces;
using Hangfire;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Emails
{
    public class EmailRankUpNotificationService(IEmailService emailService , IRazorTemplateService razorTemplateService  ) : IRankUpNotificationService
    {
        private readonly IEmailService _emailService = emailService;
        private readonly IRazorTemplateService _razorService = razorTemplateService;

        [AutomaticRetry(Attempts = 3, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendRankUpAsync(string userEmail, string userName, string newRankName)
        {
            var subject = $"Chúc mừng! Bạn đã đạt hạng {newRankName}!";

            var viewModel = new RankUpViewModel
            {
                UserName = userName,
                NewRankName = newRankName
            };

            var nameFile = "RankUp.cshtml";

            var htmlBody = await _razorService.RenderTemplateAsync(nameFile, viewModel);

            await _emailService.SendEmailAsync(userEmail, subject, htmlBody);
        }
    }
}
