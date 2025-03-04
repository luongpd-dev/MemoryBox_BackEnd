using MemoryBox.Domain.Interfaces;
using MemoryBox.Domain.Interfaces.Email;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Infrastructure.Integrations.Email
{
    public class SendMail : ISendMail
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public SendMail(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Kiểm tra địa chỉ email hợp lệ
            if (string.IsNullOrEmpty(toEmail) || !toEmail.Contains("@"))
            {
                throw new FormatException("Địa chỉ email không hợp lệ: " + toEmail);
            }

            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpUsername"], _configuration["EmailSettings:SmtpPassword"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
