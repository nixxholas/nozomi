using System.Threading.Tasks;

namespace Nozomi.Preprocessing.Events.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}