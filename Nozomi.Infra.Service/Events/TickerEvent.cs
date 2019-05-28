using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Data.ResponseModels.TickerPair;
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

        public ICollection<CurrencyTickerPair> GetCurrencyTickerPairs(string currencyAbbrv)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled
                                                  && cp.MainCurrencyAbbrv.Equals(currencyAbbrv,
                                                      StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.Source)
                .Where(cp => cp.Source.DeletedAt == null && cp.Source.IsEnabled)
                .Select(cp => new CurrencyTickerPair
                {
                    TickerPair = string.Concat(cp.MainCurrencyAbbrv, cp.CounterCurrencyAbbrv),
                    Source = cp.Source.Name
                })
                .ToList();
        }

        public Task<NozomiResult<TickerByExchangeResponse>> GetById(long id)
        {
//            return Task.FromResult(new NozomiResult<TickerByExchangeResponse>(
//                NozomiServiceConstants.CurrencyPairDictionary[id]));
            
            return Task.FromResult(new NozomiResult<TickerByExchangeResponse>()
            {
                Data = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.Id.Equals(id))
                    .Include(cp => cp.Source)
                    .Where(cp => cp.Source.IsEnabled && cp.Source.DeletedAt == null)
                    .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .Select(cp => new TickerByExchangeResponse()
                    {
                        Exchange = cp.Source.Name,
                        ExchangeAbbrv = cp.Source.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .CreatedAt,
                        Properties = cp.CurrencyPairRequests
                            .SelectMany(cpr => cpr.RequestComponents)
                            .OrderBy(rc => rc.ComponentType).Select(rc => 
                                new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                    rc.Value)).ToList()
                    })
                    .SingleOrDefault()
            });
        }

        public ICollection<TickerPairResponse> GetAllTickerPairSources()
        {
            var cPairs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.Source)
                .Where(cp => cp.Source != null && cp.Source.DeletedAt == null && cp.Source.IsEnabled);

            var res = new List<TickerPairResponse>();

            foreach (var cPair in cPairs)
            {
                var tickerPairStr = string.Concat(cPair.MainCurrencyAbbrv, cPair.CounterCurrencyAbbrv);
                
                var tPair = res.FirstOrDefault(tpair => tpair.Key.Equals(tickerPairStr,
                    StringComparison.InvariantCultureIgnoreCase));
                
                // if the ticker pair already exists in res
                if (tPair != null)
                {
                    tPair.Sources.Add(new SourceResponse
                    {
                        Abbreviation = cPair.Source.Abbreviation,
                        Name = cPair.Source.Name
                    });
                }
                else
                {
                    // Create it
                    res.Add(new TickerPairResponse
                    {
                        Key = tickerPairStr,
                        Sources = new List<SourceResponse>
                        {
                            new SourceResponse
                            {
                                Abbreviation = cPair.Source.Abbreviation,
                                Name = cPair.Source.Name
                            }
                        }
                    });
                }
            }

            return res;
        }

        public NozomiResult<ICollection<TickerByExchangeResponse>> GetByAbbreviation(string ticker, 
            string exchangeAbbrv = null)
        {
            try
            {
                if (ticker.Length != 6) return new NozomiResult<ICollection<TickerByExchangeResponse>>(
                    NozomiResultType.Failed, "Invalid Ticker Symbol."); // Invalid ticker length

                var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled 
                                                      && string.Concat(cp.MainCurrencyAbbrv, cp.CounterCurrencyAbbrv)
                                                          .Equals(ticker, StringComparison.InvariantCultureIgnoreCase))
                    .Include(cp => cp.Source)
                    .Include(cp => cp.AnalysedComponents);
                
                // Exchange? Specification.
                if (!string.IsNullOrEmpty(exchangeAbbrv))
                {
                    return new NozomiResult<ICollection<TickerByExchangeResponse>>(query
                        .Where(cp => cp.Source != null 
                                     && cp.Source.Name.Equals(exchangeAbbrv, StringComparison.InvariantCultureIgnoreCase))
                        .Select(cp => new TickerByExchangeResponse
                        {
                            Exchange = cp.Source.Name,
                            ExchangeAbbrv = cp.Source.Abbreviation,
                            LastUpdated = cp.CurrencyPairRequests
                                .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                                .RequestComponents.FirstOrDefault(rc => rc.IsEnabled && rc.DeletedAt == null)
                                .ModifiedAt,
                            Properties = cp.AnalysedComponents
                                .OrderBy(rc => rc.ComponentType).Select(rc => 
                                    new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                        rc.Value)).ToList()
                        })
                        .ToList());
                }

                return new NozomiResult<ICollection<TickerByExchangeResponse>>(query
                    .Select(cp => new TickerByExchangeResponse
                    {
                        Exchange = cp.Source.Name,
                        ExchangeAbbrv = cp.Source.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests
                            .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                            .RequestComponents.FirstOrDefault(rc => rc.IsEnabled && rc.DeletedAt == null)
                            .ModifiedAt,
                        Properties = cp.AnalysedComponents
                            .OrderBy(rc => rc.ComponentType).Select(rc => 
                                new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                    rc.Value)).ToList()
                    })
                    .ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new NozomiResult<ICollection<TickerByExchangeResponse>>()
                {
                    ResultType = NozomiResultType.Failed,
                    Message = "An error has occurred.",
                    Data = null
                }; 
            }
        }
    }
}