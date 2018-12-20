using System.Threading.Tasks;

namespace Nozomi.Preprocessing.Events.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}