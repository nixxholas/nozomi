using System.Threading.Tasks;
using Nozomi.Base.Core.Commands;
using Nozomi.Base.Core.Events;

namespace Nozomi.Base.Core.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}