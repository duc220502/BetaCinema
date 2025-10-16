using BetaCinema.Application.DTOs;
using BetaCinema.Application.Interfaces;
using BetaCinema.Infrastructure.Configuration;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Emails
{
    public class EmailAlertingService (IEmailService emailService , IOptions<AlertingSettings> alertingSettings , IRazorTemplateService razorTemplateService ) : IAlertingService
    {

        private readonly IEmailService _emailService = emailService;
        private readonly AlertingSettings _alertingSettings = alertingSettings.Value;
        private readonly IRazorTemplateService _razorService = razorTemplateService;

        [AutomaticRetry(Attempts = 3, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendAdminAlertAsync(JobFailedAlertDto viewModel)
        {
            var subject = $"Cảnh báo: Hangfire Job {viewModel.JobName} đã thất bại vĩnh viễn !";
            var nameFile = "JobFailedAlert.cshtml";

            await SendAlertInternalAsync(subject, nameFile, viewModel);
        }

        [AutomaticRetry(Attempts = 3, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendReconciliationFailedAlertAsync(ReconciliationFailedAlertViewModel viewModel)
        {
            var subject = $"[KHẨN CẤP] Lỗi Đối soát Thanh toán cho Hóa đơn {viewModel.BillId}";
            var nameFile = "ReconciliationFailedAlert.cshtml";

            await SendAlertInternalAsync(subject, nameFile, viewModel);
        }

        public async Task SendSecurityAlertAsync(SecurityAlertViewModel viewModel)
        {
            var subject = $"[BẢO MẬT KHẨN CẤP] Phát hiện IPN đáng ngờ cho Hóa đơn {viewModel.BillId}";
            var nameFile = "SecurityAlert.cshtml";
            

            await SendAlertInternalAsync(subject, nameFile, viewModel);
        }

        private async Task SendAlertInternalAsync<TViewModel>(string subject, string nameFile, TViewModel viewModel)
        {
            var adminEmail = _alertingSettings.AdminEmailRecipient;

            var htmlBody = await _razorService.RenderTemplateAsync(nameFile, viewModel);


            if (!string.IsNullOrEmpty(adminEmail))
            {
                await _emailService.SendEmailAsync(adminEmail, subject, htmlBody);
            }
         
        }
   }

}
