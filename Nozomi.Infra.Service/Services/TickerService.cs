using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Native.Collections;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.WebModels;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class TickerService : BaseService<TickerService, NozomiDbContext>, ITickerService
    {
        public TickerService(ILogger<TickerService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public Task<NozomiResult<ICollection<UniqueTickerResponse>>> Get(int index)
        {
            return Task.FromResult(new NozomiResult<ICollection<UniqueTickerResponse>>
            {
                Data = _unitOfWork.GetRepository<CurrencyPair>()
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
                        TickerAbbreviation = cp.PartialCurrencyPairs.SingleOrDefault(pcp => pcp.IsMain).ToString()
                        + cp.PartialCurrencyPairs.SingleOrDefault(pcp => !pcp.IsMain).ToString(),
                        Exchange = cp.CurrencySource.Name,
                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
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
                    .ToList()
            });
        }

        public Task<NozomiResult<DistinctiveTickerResponse>> GetById(long id)
        {
            return Task.FromResult(new NozomiResult<DistinctiveTickerResponse>(
                NozomiServiceConstants.CurrencyPairDictionary[id]));
            
//            return Task.FromResult(new NozomiResult<TickerResponse>()
//            {
//                Data = _unitOfWork.GetRepository<CurrencyPair>()
//                    .GetQueryable()
//                    .AsNoTracking()
//                    .Where(cp => cp.Id.Equals(id))
//                    .Include(cp => cp.CurrencyPairRequests)
//                    .ThenInclude(cpr => cpr.RequestComponents)
//                    .ThenInclude(rc => rc.RequestComponentDatum)
//                    .Select(cp => new TickerResponse()
//                    {
//                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
//                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
//                            .RequestComponentDatum
//                            .CreatedAt,
//                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
//                            .RequestComponents
//                            .Select(rc => new KeyValuePair<string, string>(
//                                rc.ComponentType.ToString(), 
//                                rc.RequestComponentDatum.Value))
//                            .ToList()
//                                    
//                    })
//                    .SingleOrDefault()
//            });
        }

        public NozomiResult<ICollection<DistinctiveTickerResponse>> GetByAbbreviation(string ticker, string exchangeAbbrv = null)
        {
            try
            {
                if (ticker.Length != 6) return null; // Invalid ticker length
                
                return new NozomiResult<ICollection<DistinctiveTickerResponse>>(
                    NozomiServiceConstants.TickerSymbolDictionary[ticker].Where(i => i > 0).Select(
                    i => NozomiServiceConstants.CurrencyPairDictionary[i]).ToList());

//                var query = _unitOfWork.GetRepository<CurrencyPair>()
//                    .GetQueryable()
//                    .AsNoTracking()
//                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
//                    .Include(cp => cp.PartialCurrencyPairs)
//                    .ThenInclude(pcp => pcp.Currency)
//                    .Where(cp => (cp.PartialCurrencyPairs.SingleOrDefault(pcp => pcp.IsMain).Currency
//                                      .Abbrv // Make sure the first currency (main) is equal to the ticker's first
//                                  + cp.PartialCurrencyPairs.SingleOrDefault(pcp => !pcp.IsMain).Currency.Abbrv) // other way round
//                        .Equals(ticker, StringComparison.InvariantCultureIgnoreCase))
//                    .Include(cp => cp.CurrencySource)
//                    .Where(cp => cp.CurrencySource != null) // Make sure we have a source
//                    .Include(cp => cp.CurrencyPairRequests)
//                        .ThenInclude(cpr => cpr.RequestComponents)
//                            .ThenInclude(rc => rc.RequestComponentDatum)
//                    // Make sure there's something
//                    .Where(cp => cp.CurrencyPairRequests
//                        .Any(cpr => cpr.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null && 
//                                                                    rc.RequestComponentDatum != null)))
//                    .Select(cp => new DistinctiveTickerResponse()
//                    {
//                        Exchange = cp.CurrencySource.Name,
//                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
//                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault()
//                            .RequestComponents.FirstOrDefault()
//                            .RequestComponentDatum.ModifiedAt,
//                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
//                            .RequestComponents
//                            .Select(rc => new KeyValuePair<string, string>(
//                                rc.ComponentType.ToString(), 
//                                rc.RequestComponentDatum.Value))
//                            .ToList()
//                    });
//
//                // Exchange-based filter
//                if (!exchangeAbbrv.IsNullOrEmpty())
//                {
//                    query = query.Where(cp => cp.ExchangeAbbrv.Equals(exchangeAbbrv));
//                }
//
//                return new NozomiResult<ICollection<DistinctiveTickerResponse>>()
//                {
//                    ResultType = NozomiResultType.Success,
//                    Data = query.ToList()
//                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new NozomiResult<ICollection<DistinctiveTickerResponse>>()
                {
                    ResultType = NozomiResultType.Failed,
                    Message = "An error has occurred.",
                    Data = null
                }; 
            }
        }

        public NozomiResult<ICollection<DistinctiveTickerResponse>> GetAllActive()
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.PartialCurrencyPairs)
                    .ThenInclude(pcp => pcp.Currency)
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
                    })
                    .ToList();
            
            return new NozomiResult<ICollection<DistinctiveTickerResponse>>()
            {
                ResultType = (query != null) ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = (query != null) ? query : new List<DistinctiveTickerResponse>()
            };
        }
    }
}