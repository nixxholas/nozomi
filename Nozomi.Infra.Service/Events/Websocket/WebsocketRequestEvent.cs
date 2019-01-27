using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.WebsocketModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Websocket.Interfaces;

namespace Nozomi.Service.Events.Websocket
{
    public class WebsocketRequestEvent : BaseEvent<WebsocketRequestEvent, NozomiDbContext>, IWebsocketRequestEvent
    {
        public WebsocketRequestEvent(ILogger<WebsocketRequestEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }
        
        private static readonly Func<NozomiDbContext, RequestType, IEnumerable<WebsocketRequest>> 
            GetWebsocketRequestByRequestType =
            EF.CompileQuery((NozomiDbContext context, RequestType type) =>
                context.WebsocketRequests
                    .AsQueryable()
                    .Include(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .Include(r => r.CurrencyPair)
                    .Include(r => r.RequestProperties)
                    .Include(r => r.WebsocketCommands)
                    .ThenInclude(wsc => wsc.WebsocketCommandProperties)
                    .Where(r => r.IsEnabled && r.DeletedAt == null
                                            && r.RequestType == type
                                            && r.RequestComponents.Any(rc => rc.RequestComponentDatum == null
                                                                             || (DateTime.UtcNow > (rc.RequestComponentDatum
                                                                                     .CreatedAt.Add(TimeSpan.FromMilliseconds(r.Delay)))))));

        public ICollection<WebsocketRequest> GetAllByRequestType(RequestType requestType)
        {
            return GetWebsocketRequestByRequestType(_unitOfWork.Context, requestType).ToList();
        }
    }
}