using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.WebModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.HostedServices.Interfaces;

namespace Nozomi.Service.Identity.HostedServices
{
    public class ApiTokenCachingService : BaseHostedService<ApiTokenCachingService>,
        IApiTokenCachingService, IHostedService, IDisposable
    {
        private readonly NozomiAuthContext _context;
        private IDistributedCache _cache;
        
        public ApiTokenCachingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = _scope.ServiceProvider.GetRequiredService<NozomiAuthContext>();
            _cache = _scope.ServiceProvider.GetService<IDistributedCache>();
        }
        
        private static readonly Func<NozomiAuthContext, IEnumerable<ApiToken>> 
            GetActiveApiTokens =
                EF.CompileQuery((NozomiAuthContext context) =>
                    context.ApiTokens
                        .AsQueryable()
                        .AsNoTracking()
                        .Where(at => at.IsEnabled && at.DeletedAt == null));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            _logger.LogInformation("ApiTokenCachingService is starting.");

            stoppingToken.Register(
                () => _logger.LogInformation("ApiTokenCachingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var activeApiTokens = GetActiveApiTokens(_context);

                    // Iterate the requests
                    foreach (var token in activeApiTokens)
                    {
                        var cachedTokenBytes = _cache.Get(token.Guid.ToString());
                        
                        // If the item exists
                        if (cachedTokenBytes != null)
                        {
                            ApiToken cachedApiToken;
                            using (var stream = new MemoryStream(cachedTokenBytes)) {
                                cachedApiToken = new BinaryFormatter().Deserialize(stream) as ApiToken;
                            }

                            if (cachedApiToken != null 
                                && (cachedApiToken.ModifiedAt < token.ModifiedAt))
                            {
                                // Update
                                using (var stream = new MemoryStream())
                                {
                                    new BinaryFormatter().Serialize(stream, token);
                                    var bytes = stream.ToArray();
                                    _cache.Set(token.Guid.ToString(), bytes);
                                }
                            }
                        }
                        else
                        {
                            // Delete
                            _cache.Remove(token.Guid.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[ApiTokenCachingService]: " + ex);
                }

                // .5s nap
                await Task.Delay(500, stoppingToken);
            }

            _logger.LogWarning("ApiTokenCachingService background task is stopping.");
        }
    }
}