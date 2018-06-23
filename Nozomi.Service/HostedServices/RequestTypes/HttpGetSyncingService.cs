using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class HttpGetSyncingService : BaseHostedService, IHttpGetSyncingService
    {
        private readonly IRequestService _requestService;
        private readonly IRequestLogService _requestLogService;
        private readonly ILogger<HttpGetSyncingService> _logger;
        private List<Request> _requestList;
        
        public HttpGetSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _requestService = _scope.ServiceProvider.GetRequiredService<RequestService>();
            _requestLogService = _scope.ServiceProvider.GetRequiredService<RequestLogService>();
            
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<HttpGetSyncingService>>();

            // Initialize the request list for all GET requests
            _requestList = _requestService.GetAllActive(true)
                               .Where(r => r.RequestType.Equals(RequestType.HttpGet))
                               .ToList() ?? new List<Request>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            _logger.LogInformation("HttpGetSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("HttpGetSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
                var getBasedRequests = _requestService.GetAll(r => r.IsEnabled && r.DeletedAt == null
                                                                               && r.RequestType.Equals(RequestType
                                                                                   .HttpGet), true);

                // Iterate the requests
                // NOTE: Let's not call a parallel loop since HttpClients might tend to result in memory leaks.
                foreach (var rq in getBasedRequests)
                {
                    // Process the request
                    if (Process(rq))
                    {
                        // Since its successful
                    }
                    else
                    {
                        // Since its unsuccessful
                        
                        // log it.
                    }
                }
            }

            _logger.LogWarning("HttpGetSyncingService background task is stopping.");
        }

        public bool Process(Request req)
        {
            throw new NotImplementedException();
        }
    }
}