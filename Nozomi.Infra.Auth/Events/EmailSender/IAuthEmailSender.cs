using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Events.EmailSender
{
    public interface IAuthEmailSender
    {
        Task SendEmailConfirmationAsync (string email, string callbackUrl);

        Task SendPasswordResetLinkAsync(string email, string callbackUrl);
    }
}