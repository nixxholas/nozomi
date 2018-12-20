using System.Threading.Tasks;
using Nozomi.Preprocessing.Events.Interfaces;

namespace Nozomi.Preprocessing.Events
{
    public class SmsSender : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}