using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Infra.Preprocessing.SignalR;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Hubs;

namespace Nozomi.Service.HostedServices.StaticUpdater
{
    public class SourceSyncingService : BaseHostedService<SourceSyncingService>, IHostedService, IDisposable
    {
        private readonly NozomiDbContext _nozomiDbContext;

        private static readonly Func<NozomiDbContext, IEnumerable<Source>>
            GetSourceResponse =
                EF.CompileQuery((NozomiDbContext context) =>
                    context.Sources
                        .AsQueryable()
                        .Where(s => s.IsEnabled && s.DeletedAt == null)
                        .Include(s => s.CurrencyPairs)
                            .ThenInclude(cp => cp.PartialCurrencyPairs)
                                .ThenInclude(pcp => pcp.Currency)
                        .Include(s => s.CurrencyPairs)
                            .ThenInclude(cp => cp.CurrencyPairRequests)
                        // Historical Data Inclusions
                        .Include(s => s.Currencies)
                        .ThenInclude(c => c.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                        .ThenInclude(rcd => rcd.RcdHistoricItems)
                    );
        
        private readonly IHubContext<NozomiSourceStreamHub, ISourceHubClient> _nozomiSourceStreamHub;
        
        public SourceSyncingService(IServiceProvider serviceProvider, 
            IHubContext<NozomiSourceStreamHub, ISourceHubClient> nozomiSourceStreamHub) : base(serviceProvider)
        {
            _nozomiDbContext = _scope.ServiceProvider.GetService<NozomiDbContext>();
            _nozomiSourceStreamHub = nozomiSourceStreamHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SourceSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("SourceSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    NozomiServiceConstants.Sources = GetSourceResponse(_nozomiDbContext);

                    foreach (var source in NozomiServiceConstants.Sources)
                    {
                        await _nozomiSourceStreamHub.Clients
                            .Group(NozomiSourceStreamHub.NozomiSourceStreamHubStr + source.Abbreviation)
                            .Tickers(source.CurrencyPairs
                                .Select(cp => new UniqueTickerResponse
                                {
                                    MainTickerAbbreviation = 
                                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain)?.Currency.Abbrv,
                                    MainTickerName = 
                                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain)?.Currency.Name,
                                    CounterTickerAbbreviation = 
                                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain)?.Currency.Abbrv,
                                    CounterTickerName = 
                                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain)?.Currency.Name,
                                    CurrencyPairId = cp.Id,
                                    LastUpdated = cp.ModifiedAt,
                                    Properties = cp.CurrencyPairRequests
                                        .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                                        ?.RequestComponents.OrderByDescending(rc => rc.ComponentType)
                                        .Where(rc => !string.IsNullOrEmpty(rc.Value))
                                        .Select(rc => 
                                            new KeyValuePair<string, string>(rc.QueryComponent.GetDescription(), 
                                            rc.Value))
                                        .ToList()
                                }).ToList());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[SourceSyncingService]: " + ex);
                }
                
                // No naps taken
                await Task.Delay(10, stoppingToken);
            }

            _logger.LogWarning("SourceSyncingService background task is stopping.");
        }
    }
}