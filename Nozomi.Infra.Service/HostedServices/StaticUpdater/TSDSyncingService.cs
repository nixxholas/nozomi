using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;

namespace Nozomi.Service.HostedServices.StaticUpdater
{
    public class TSDSyncingService : BaseHostedService<TSDSyncingService>, IHostedService, IDisposable
    {
        private NozomiDbContext _nozomiDbContext;
        
        private static readonly Func<NozomiDbContext, IEnumerable<CurrencyPair>> 
            GetActiveCurrencyPairs =
                EF.CompileQuery((NozomiDbContext context) =>
                    context.CurrencyPairs
                        .AsNoTracking()
                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                        .Include(cp => cp.CurrencySource)
                        .Include(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                        .Include(cp => cp.CurrencyPairRequests)
                        .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
                                     && cpr.RequestComponents.Any(rc => rc.DeletedAt == null && rc.IsEnabled))
                                     && cp.PartialCurrencyPairs.Count == 2));
        
        public TSDSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _nozomiDbContext = _scope.ServiceProvider.GetService<NozomiDbContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TSDSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("TSDSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cPairs = GetActiveCurrencyPairs(_nozomiDbContext).ToList();
                    
                    // Populate a new TickerSymbolDictionary
                    var newTsd = new Dictionary<string, LinkedList<long>>();

                    foreach (var cp in cPairs)
                    {
                        var tickerSymbol = string.Join("", cp.PartialCurrencyPairs
                            .OrderByDescending(pcp => pcp.IsMain)
                            .Select(pcp => pcp.Currency.Abbrv));
                        
                        // Check if the ticker exists in the collection
                        if (newTsd.ContainsKey(tickerSymbol))
                        {
                            var longList = newTsd[tickerSymbol];

                            // Make sure the list already has it
                            if (!longList.Contains(cp.Id))
                            {
                                longList.AddLast(cp.Id);
                            }
                        }
                        else // nope, let's add it in
                        {
                            newTsd.Add(tickerSymbol,
                                new LinkedList<long>());

                            newTsd[tickerSymbol].AddLast(cp.Id);
                        }
                    }

                    // Complete replace
                    NozomiServiceConstants.TickerSymbolDictionary = newTsd;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[TSDSyncingService]: " + ex);
                }
                
                // No naps taken
                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogWarning("TSDSyncingService background task is stopping.");
        }
    }
}