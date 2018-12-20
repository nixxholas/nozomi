using System.Threading.Tasks;
using Nozomi.Preprocessing.Events.Interfaces;

namespace Nozomi.Preprocessing.Events
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}