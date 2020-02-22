using System;
using System.Threading;
using System.Threading.Tasks;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Compute.Abstracts
{
    public abstract class BaseComputeService<T> : BaseHostedService<T> where T : class
    {
        public readonly string _computeServiceName;
        
        protected BaseComputeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _computeServiceName = typeof(T).FullName;
        }
    }
}