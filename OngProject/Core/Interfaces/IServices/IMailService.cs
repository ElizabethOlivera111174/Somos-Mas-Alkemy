using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(string ToEmail, string body, string subject);
    }
}
