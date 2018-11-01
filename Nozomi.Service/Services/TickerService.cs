using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Core.Helpers.Native.Collections;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class TickerService : BaseService<TickerService, NozomiDbContext>, ITickerService
    {
        public TickerService(ILogger<TickerService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public Task<NozomiResult<TickerResponse>> GetById(long id)
        {
            return Task.FromResult(new NozomiResult<TickerResponse>()
            {
                Data = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.Id.Equals(id))
                    .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .Select(cp => new TickerResponse()
                    {
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .RequestComponentDatum
                            .CreatedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents
                            .Select(rc => new KeyValuePair<string, string>(
                                rc.ComponentType.ToString(), 
                                rc.RequestComponentDatum.Value))
                            .ToList()
                                    
                    })
                    .SingleOrDefault()
            });
        }

        public NozomiResult<ICollection<DistinctiveTickerResponse>> GetByAbbreviation(string ticker, string exchangeAbbrv = null)
        {
            try
            {
                if (ticker.Length != 6) return null; // Invalid ticker length

                var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.PartialCurrencyPairs)
                    .ThenInclude(pcp => pcp.Currency)
                    .Where(cp => (cp.PartialCurrencyPairs.SingleOrDefault(pcp => pcp.IsMain).Currency
                                      .Abbrv // Make sure the first currency (main) is equal to the ticker's first
                                  + cp.PartialCurrencyPairs.SingleOrDefault(pcp => !pcp.IsMain).Currency.Abbrv) // other way round
                        .Equals(ticker, StringComparison.InvariantCultureIgnoreCase))
                    .Include(cp => cp.CurrencySource)
                    .Where(cp => cp.CurrencySource != null) // Make sure we have a source
                    .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                            .ThenInclude(rc => rc.RequestComponentDatum)
                    // Make sure there's something
                    .Where(cp => cp.CurrencyPairRequests
                        .Any(cpr => cpr.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null && 
                                                                    rc.RequestComponentDatum != null)))
                    .Select(cp => new DistinctiveTickerResponse()
                    {
                        Exchange = cp.CurrencySource.Name,
                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents.FirstOrDefault()
                            .RequestComponentDatum.ModifiedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents
                            .Select(rc => new KeyValuePair<string, string>(
                                rc.ComponentType.ToString(), 
                                rc.RequestComponentDatum.Value))
                            .ToList()
                    });

                // Exchange-based filter
                if (!exchangeAbbrv.IsNullOrEmpty())
                {
                    query = query.Where(cp => cp.ExchangeAbbrv.Equals(exchangeAbbrv));
                }

                return new NozomiResult<ICollection<DistinctiveTickerResponse>>()
                {
                    ResultType = NozomiResultType.Success,
                    Data = query.ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null; 
            }
        }
    }
}