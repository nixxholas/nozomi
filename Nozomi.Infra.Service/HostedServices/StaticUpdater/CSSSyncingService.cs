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
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;

namespace Nozomi.Service.HostedServices.StaticUpdater
{
    public class CSSSyncingService : BaseHostedService<CSSSyncingService>, IHostedService, IDisposable
    {
        private readonly NozomiDbContext _nozomiDbContext;
        
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
        
        public CSSSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _nozomiDbContext = _scope.ServiceProvider.GetService<NozomiDbContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CSSSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("CSSSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var currencyPairs = GetActiveCurrencyPairs(_nozomiDbContext).ToList();

                    foreach (var cp in currencyPairs)
                    {
                        var tickerSymbol = string.Join("", cp.PartialCurrencyPairs
                            .OrderByDescending(pcp => pcp.IsMain)
                            .Select(pcp => pcp.Currency.Abbrv));

                        var dictKey = new Tuple<string, string>(cp.CurrencySource.Abbreviation, tickerSymbol);

                        if (!NozomiServiceConstants.CurrencySourceSymbolDictionary.ContainsKey(dictKey))
                        {
                            // Populate the CurrencySourceSymbolDictionary
                            NozomiServiceConstants.CurrencySourceSymbolDictionary.Add(dictKey, cp.Id);
                        }
                    } 
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[CSSSyncingService]: " + ex);
                }
                
                // No naps taken
                await Task.Delay(10000, stoppingToken);
            }

            _logger.LogWarning("CSSSyncingService background task is stopping.");
        }
    }
}