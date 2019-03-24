using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Websocket;
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
                    .Include(r => r.CurrencyPair)
                    .Include(r => r.RequestProperties)
                    .Include(r => r.WebsocketCommands)
                    .ThenInclude(wsc => wsc.WebsocketCommandProperties)
                    .Where(r => r.IsEnabled && r.DeletedAt == null
                                            && r.RequestType == type
                                            && r.RequestComponents.Any(rc => (DateTime.UtcNow > (rc.ModifiedAt.Add(TimeSpan.FromMilliseconds(r.Delay)))))));

        public ICollection<WebsocketRequest> GetAllByRequestType(RequestType requestType)
        {
            return GetWebsocketRequestByRequestType(_unitOfWork.Context, requestType).ToList();
        }

        public IDictionary<string, ICollection<WebsocketRequest>> GetAllByRequestTypeUniqueToURL(RequestType requestType)
        {
            var dict = new Dictionary<string, ICollection<WebsocketRequest>>();
            var websocketRequests = GetWebsocketRequestByRequestType(_unitOfWork.Context, requestType);

            foreach (var websocketRequest in websocketRequests)
            {
                // If the key exists,
                if (dict.ContainsKey(websocketRequest.DataPath) && dict[websocketRequest.DataPath] != null 
                                                        && dict[websocketRequest.DataPath].Count > 0)
                {
                    dict[websocketRequest.DataPath].Add(websocketRequest);
                }
                // If not create it
                else
                {
                    dict.Add(websocketRequest.DataPath, new List<WebsocketRequest>());
                    dict[websocketRequest.DataPath].Add(websocketRequest);
                }
            }

            return dict;
        }
    }
}