using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyRequestEvent : BaseEvent<CurrencyRequestEvent, NozomiDbContext>, ICurrencyRequestEvent
    {
        public CurrencyRequestEvent(ILogger<CurrencyRequestEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        // CurrencyRequest obtainability
        private static readonly Func<NozomiDbContext, RequestType, IEnumerable<CurrencyRequest>> 
            GetActiveCurrencyRequests =
                EF.CompileQuery((NozomiDbContext context, RequestType requestType) =>
                    context.CurrencyRequests
                        .Where(cr => cr.DeletedAt == null && cr.IsEnabled && 
                                     cr.RequestType.Equals(requestType))
                        .Include(cr => cr.RequestComponents)
                        .ThenInclude(rc => rc.RequestComponentDatum)
                        .Include(cr => cr.RequestProperties));
        
        public IDictionary<string, ICollection<CurrencyRequest>> GetAllByRequestTypeUniqueToUrl(RequestType requestType)
        {
            var dict = new Dictionary<string, ICollection<CurrencyRequest>>();
            var currencyRequests = GetActiveCurrencyRequests(_unitOfWork.Context, requestType);

            foreach (var cReq in currencyRequests)
            {
                // If the key exists,
                if (dict.ContainsKey(cReq.DataPath) && dict[cReq.DataPath] != null 
                                                        && dict[cReq.DataPath].Count > 0)
                {
                    dict[cReq.DataPath].Add(cReq);
                }
                // If not create it
                else
                {
                    dict.Add(cReq.DataPath, new List<CurrencyRequest>());
                    dict[cReq.DataPath].Add(cReq);
                }
            }

            return dict;
        }
    }
}