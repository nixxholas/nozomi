using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.WebModels;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Hubs.Enumerators;
using Nozomi.Repo.Data;
using Nozomi.Service.Hubs;
using Nozomi.Service.Hubs.Interfaces;

namespace Nozomi.Service.HostedServices.StaticUpdater
{
    /// <summary>
    /// CurrencyPairDictionarySyncingService.
    /// </summary>
    public class CPDSyncingService : BaseHostedService<CPDSyncingService>,
        IHostedService, IDisposable
    {
        private readonly NozomiDbContext _nozomiDbContext;
        private readonly IHubContext<TickerHub, ITickerHubClient> _tickerHub;
        
        private static readonly Func<NozomiDbContext, IEnumerable<DiscoverabeTickerResponse>> 
            GetActiveDiscoverableTickerResponses =
            EF.CompileQuery((NozomiDbContext context) =>
                context.CurrencyPairRequests
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(r => r.DeletedAt == null && r.IsEnabled)
                    .Include(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .Include(r => r.CurrencyPair)
                    .ThenInclude(cp => cp.CurrencySource)
                    .Where(r => r.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null
                                                                           && rc.RequestComponentDatum != null))
                    .Select(cpr => new DiscoverabeTickerResponse()
                    {
                        CurrencyPairId = cpr.CurrencyPairId,
                        Exchange = cpr.CurrencyPair.CurrencySource.Name,
                        ExchangeAbbrv = cpr.CurrencyPair.CurrencySource.Abbreviation,
                        LastUpdated = cpr.RequestComponents.First().RequestComponentDatum.ModifiedAt,
                        Properties = cpr.RequestComponents.Select(rc => 
                            new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                rc.RequestComponentDatum.Value)).ToList()
                    }));
        
        public CPDSyncingService(IServiceProvider serviceProvider,
            IHubContext<TickerHub, ITickerHubClient> tickerHub) : base(serviceProvider)
        {
            _nozomiDbContext = _scope.ServiceProvider.GetService<NozomiDbContext>();
            _tickerHub = tickerHub;
        }

        /// <summary>
        /// Synchronizes NozomiServiceConstants.CurrencyPairDictionary with its database-driven counterpart.
        ///
        /// This syncing service does not remove any deprecated data, this is not required since it disappears
        /// after a server update and we have value timestamps to keep the user on check for deprecated data.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CPDSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("CPDSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var dtrList = GetActiveDiscoverableTickerResponses(_nozomiDbContext).ToList();
                    
                    foreach (var dtr in dtrList)
                    {
                        // Update if it exists
                        if (NozomiServiceConstants.CurrencyPairDictionary.ContainsKey(dtr.CurrencyPairId))
                        {
                            NozomiServiceConstants.CurrencyPairDictionary[dtr.CurrencyPairId] = dtr;
                        }
                        else
                        {
                            // Add it if it doesn't
                            NozomiServiceConstants.CurrencyPairDictionary.Add(dtr.CurrencyPairId, dtr);
                        }
                    }
                    
                    _tickerHub.Clients.All.BroadcastData(TickerHubGroup.Ticker);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[CPDSyncingService]: " + ex);
                }
                
                // No naps taken
                await Task.Delay(100, stoppingToken);
            }

            _logger.LogWarning("CPDSyncingService background task is stopping.");
        }
    }
}