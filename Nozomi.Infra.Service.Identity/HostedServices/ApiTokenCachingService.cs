using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Identity.HostedServices.Interfaces;

namespace Nozomi.Service.Identity.HostedServices
{
    public class ApiTokenCachingService : BaseHostedService<ApiTokenCachingService>,
        IApiTokenCachingService, IHostedService, IDisposable
    {
        public ApiTokenCachingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}