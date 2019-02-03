using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class TickerEvent : BaseEvent<TickerEvent, NozomiDbContext>, ITickerEvent
    {
        public TickerEvent(ILogger<TickerEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public DataTableResult<UniqueTickerResponse> GetAllForDatatable(int index = 0)
        {
            var data = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Skip(index * 20)
                .Take(20)
                .OrderBy(cp => cp.Id)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Include(cp => cp.CurrencySource)
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .ThenInclude(rc => rc.RequestComponentDatum)
                .Select(cp => new UniqueTickerResponse
                {
                    TickerAbbreviation = string.Concat(
                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv,
                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv),
                    Exchange = cp.CurrencySource.Name,
                    ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                    LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                        .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                        .RequestComponentDatum
                        .ModifiedAt,
                    Properties = cp.CurrencyPairRequests.FirstOrDefault()
                        .RequestComponents
                        .Select(rc => new KeyValuePair<string, string>(
                            rc.ComponentType.ToString(),
                            rc.RequestComponentDatum.Value))
                        .ToList()
                })
                .ToList();
            var fullCount = _unitOfWork.GetRepository<CurrencyPair>().GetQueryable().Count();
            
            return new DataTableResult<UniqueTickerResponse>
            {
                Draw = index,
                RecordsTotal = fullCount,
                RecordFiltered = fullCount - data.Count,
                Data = data
            };
        }
    }
}