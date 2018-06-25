using Nozomi.Data.WebModels;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class HttpPostCurrencyPairRequestSyncingService : BaseHostedService, IHttpPostCurrencyPairRequestSyncingService
    {
        public HttpPostCurrencyPairRequestSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<bool> Process(CurrencyPairRequest req)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
