using System;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
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
            if (dispatchInputModel != null && dispatchInputModel.IsValid())
            {
                switch (dispatchInputModel.Type)
                {
                    case RequestType.HttpGet:
                        break;
                    case RequestType.HttpPost:
                        break;
                    case RequestType.WebSocket:
                        break;
                    default:
                        throw new InvalidOperationException("Invalid protocol type.");
                }
            }

            throw new NullReferenceException("Invalid payload.");
        }
    }
}