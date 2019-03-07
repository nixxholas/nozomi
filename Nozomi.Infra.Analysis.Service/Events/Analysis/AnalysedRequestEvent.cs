using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Analysis.Base.Domain.Responses.Hub.Asset;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis
{
    public class AnalysedRequestEvent : BaseEvent<AnalysedRequestEvent, NozomiDbContext>, 
        IAnalysedRequestEvent
    {
        public AnalysedRequestEvent(ILogger<AnalysedRequestEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<AssetResponse> GetAll(long index = 0, bool active = true)
        {
            // Obtain everything we need first
            var unmergedCurrencies = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                // Make sure we're not using disabled or deleted currency pairs
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled.Equals(active))
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                // Make sure we're not using deleted or disabled currencies
                .Where(cp => cp.PartialCurrencyPairs
                    .Any(pcp => pcp.Currency.IsEnabled && pcp.Currency.DeletedAt == null))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents);
            
            throw new System.NotImplementedException();
        }
    }
}