using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Preprocessing.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nozomi.Preprocessing.Events
{
    public class EmailSender : IEmailSender
    {
        public SendgridOptions Options { get; }
        
        public EmailSender(IOptions<SendgridOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        
        public Task SendEmailAsync(string email, string subject, string message)
         {
             return Execute(subject, message, email);
         }
         
         public Task Execute(string subject, string message, string email)
         {
             var client = new SendGridClient(Options.SendGridKey);
             var msg = new SendGridMessage()
             {
                 From = new EmailAddress("noreply@nozomi.io", "Nozomi"),
                 Subject = subject,
                 PlainTextContent = message,
                 HtmlContent = message
             };
             msg.AddTo(new EmailAddress(email));
 
             // Disable click tracking.
             // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
             msg.SetClickTracking(false, false);
 
             return client.SendEmailAsync(msg);
         }
     }
 }