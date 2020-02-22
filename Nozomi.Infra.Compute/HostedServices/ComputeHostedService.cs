using System;
using System.Threading;
using System.Threading.Tasks;
using Nozomi.Infra.Compute.Abstracts;

namespace Nozomi.Infra.Compute.HostedServices
{
    public class ComputeHostedService : BaseComputeService<ComputeHostedService>
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