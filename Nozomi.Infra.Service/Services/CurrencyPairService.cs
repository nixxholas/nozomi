using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairService : BaseService<CurrencyPairService, NozomiDbContext>, ICurrencyPairService
    {
        public CurrencyPairService(ILogger<CurrencyPairService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger,
            unitOfWork)
        {
        }

        [Obsolete]
        public bool Create(CreateCurrencyPair createCurrencyPair, long userId)
        {
            if (createCurrencyPair == null || !createCurrencyPair.IsValid()) return false;

            var currencyPair = new CurrencyPair()
            {
                CurrencyPairType = createCurrencyPair.CurrencyPairType,
                APIUrl = createCurrencyPair.ApiUrl,
                DefaultComponent = createCurrencyPair.DefaultComponent,
                SourceId = createCurrencyPair.CurrencySourceId,
                CurrencyPairRequests = createCurrencyPair.CurrencyPairRequests
                    .Select(cpr => new CurrencyPairRequest()
                    {
                        RequestType = cpr.RequestType,
                        DataPath = cpr.DataPath,
                        Delay = cpr.Delay,
                        RequestComponents = cpr.RequestComponents
                            .Select(rc => new RequestComponent()
                            {
                                ComponentType = rc.ComponentType,
                                QueryComponent = rc.QueryComponent
                            })
                            .ToList(),
                        RequestProperties = cpr.RequestProperties
                            .Select(rp => new RequestProperty()
                            {
                                RequestPropertyType = rp.RequestPropertyType,
                                Key = rp.Key,
                                Value = rp.Value
                            })
                            .ToList()
                    })
                    .ToList()
            };
            
            if (userId > 0)
            {
                _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
                _unitOfWork.Commit(userId);

                return true;
            }
            else
            {
                _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
                _unitOfWork.Commit();

                return true;
            }
        }

        public IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false)
        {
            return !includeNested ? _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Skip(index * 20)
                .Take(20) :
                _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Skip(index * 20)
                    .Take(20);
        }

        public IEnumerable<string> GetAllCurrencyPairUrls()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Select(cp => cp.APIUrl)
                .ToList();
        }

        public long GetCPairIdByTrio(long walletTypeId, long currencyId, long currencySourceId)
        {
            var cPair = _unitOfWork
                .GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.SourceId.Equals(currencySourceId))
                .FirstOrDefault(cp => cp.CurrencyPairCurrencies // CurrencyId is the counterpair
                                          .Any(pcp => pcp.CurrencyId.Equals(currencyId) &&
                                                      pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.CounterCurrency,
                                                          StringComparison.InvariantCultureIgnoreCase)) &&
                                      cp.CurrencyPairCurrencies.Any(pcp =>
                                          pcp.Currency.WalletTypeId.Equals(walletTypeId) &&
                                          pcp.CurrencyPair.CurrencySourceId.Equals(currencySourceId)));

            if (cPair != null)
                return cPair.Id;

            return -1; // bad
        }

        public ICollection<dynamic> GetAvailCPairsObsc(int index = 0, bool track = false)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking();

            // Future possible integrations
            // https://stackoverflow.com/questions/31305839/how-to-dynamically-generate-includes-in-entity-framework

            // https://stackoverflow.com/questions/8718480/c-sharp-linq-where-clause-as-a-variable
            Expression<Func<CurrencyPair, bool>> wExpression;
            //Expression<Func<CurrencyPair, bool>> iExpression; // Out for now

            wExpression = (cp => cp.DeletedAt == null && cp.IsEnabled);

            if (track)
            {
                return query
                    .Where(wExpression)
                    .Include(cp => cp.CurrencyPairCurrencies)
                    .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencySourceId = cp.SourceId,
                        currencyPairComponents = cp.CurrencyPairRequests
                            .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                            .RequestComponents
                            .Select(cpc => new
                            {
                                id = cpc.Id,
                                componentType = cpc.ComponentType,
                                value = cpc.Value,
                                isEnabled = cpc.IsEnabled
                            }),
                        partialCurrencyPairs = cp.CurrencyPairCurrencies
                            .Select(pcp => new
                            {
                                currencyId = pcp.CurrencyId,
                                currency = new
                                {
                                    abbrv = pcp.Currency.Abbreviation,
                                    name = pcp.Currency.Name,
                                    walletTypeId = pcp.Currency.WalletTypeId,
                                    isEnabled = pcp.Currency.IsEnabled
                                }
                            }),
                        defaultComponent = cp.DefaultComponent
                    })
                    .Skip(index * 20)
                    .Take(20)
                    .ToList<dynamic>();
            }
            else
            {
                return query.Where(wExpression)
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencySourceId = cp.SourceId,
                        defaultComponent = cp.DefaultComponent
                    })
                    .Skip(index * 20)
                    .Take(20)
                    .ToList<dynamic>();
            }
        }

        public long[][] GetCurrencySourceMappings()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null)
                .Where(cp => cp.IsEnabled)
                .Include(cp => cp.Source)
                .Select(cp => new long[] {cp.Id, cp.Source.Id})
                .ToArray();
        }
    }
}