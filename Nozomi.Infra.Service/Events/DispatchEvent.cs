using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Dispatch;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class DispatchEvent : BaseEvent<DispatchEvent>, IDispatchEvent
    {
        public DispatchEvent(ILogger<DispatchEvent> logger) : base(logger)
        {
        }

        public DispatchViewModel Dispatch(DispatchInputModel dispatchInputModel)
        {
            throw new System.NotImplementedException();
        }
    }
}