using System;
using System.Threading;
using System.Threading.Tasks;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Compute.HostedServices
{
    public class ComputeHostedService : BaseHostedService<ComputeHostedService>
    {
        public ComputeHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}