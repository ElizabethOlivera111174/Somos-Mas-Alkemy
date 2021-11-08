using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OngProject.Core.Interfaces.IServices;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OngProject.Core.Services
{
    public class SendGridMailService : IMailService
    {
        private IConfiguration _configuration;
        private readonly IOrganizationsServices _organizationService;

        public SendGridMailService(IConfiguration configuration, IOrganizationsServices organizationService)
        {
            _configuration = configuration; 
            _organizationService = organizationService; 
        }

        public async Task SendEmailAsync(string ToEmail, string body, string subject)
        {
           try
            {
                var ong =await _organizationService.Get();
                string html = File.ReadAllText("./Templates/htmlpage.html");
                html = html.Replace("{mail_title}", subject);
                html = html.Replace("{mail_body}", body);
                html = html.Replace("{mail_contact}", ong.Address + " <br> "+ ong.Phone);
                var apiKey = _configuration["SendGridAPIKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_configuration["VerifiedAPIMail"]);
                var to = new EmailAddress(ToEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", html);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
