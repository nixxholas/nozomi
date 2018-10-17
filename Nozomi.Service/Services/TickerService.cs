using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
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
                    .Where(cp => cp.Id.Equals(id))
                    .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentData)
                    .Select(cp => new TickerResponse()
                    {
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .RequestComponentData.FirstOrDefault(rcd => rcd.DeletedAt == null && rcd.IsEnabled)
                            .CreatedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.Where(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .ToDictionary(rc => rc.QueryComponent, rc => rc.RequestComponentData
                                .OrderByDescending(rcd => rcd.CreatedAt)
                                .FirstOrDefault(rcd => rcd.IsEnabled && rcd.DeletedAt == null).Value)
                                    
                    })
                    .SingleOrDefault()
            });
        }

        public Task<NozomiResult<ICollection<TickerResponse>>> GetTickers(string ticker)
        {
            return Task.FromResult(new NozomiResult<ICollection<TickerResponse>>()
            {
                Data = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Include(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                    .Where(cp => cp.PartialCurrencyPairs
                        .Any(pcp => pcp.IsMain && pcp.Currency.Abbrv.Equals(ticker.Take(3))
                                    && !pcp.IsMain && pcp.Currency.Abbrv.Equals(ticker.TakeLast(3))))
                    .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentData)
                    .Select(cp => new TickerResponse()
                    {
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .RequestComponentData.FirstOrDefault(rcd => rcd.DeletedAt == null && rcd.IsEnabled)
                            .CreatedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.Where(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .ToDictionary(rc => rc.QueryComponent, rc => rc.RequestComponentData
                                .OrderByDescending(rcd => rcd.CreatedAt)
                                .FirstOrDefault(rcd => rcd.IsEnabled && rcd.DeletedAt == null).Value)
                                    
                    })
                    .ToList()
            });
        }
    }
}