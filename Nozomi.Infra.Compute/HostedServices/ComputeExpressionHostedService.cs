using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Abstracts;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Infra.Compute.HostedServices
{
    public class ComputeExpressionHostedService : BaseComputeService<ComputeExpressionHostedService>
    {
        public ComputeExpressionHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var computeExpressionEvent = _scope.ServiceProvider.GetRequiredService<IComputeExpressionEvent>();

                var mostOutdatedExps = computeExpressionEvent.GetByAge()
                    .Where(e => !e.Type.Equals(ComputeExpressionType.Generic))
                    .ToList();

                if (mostOutdatedExps.Any())
                {
                    foreach (var exp in mostOutdatedExps)
                    {
                        switch (exp.Type)
                        {
                            case ComputeExpressionType.Raw:
                                var componentHistoricItemEvent = _scope.ServiceProvider
                                    .GetRequiredService<IComponentHistoricItemEvent>();

                                var lastRawValue = componentHistoricItemEvent
                                    .GetLastItem(exp.Expression);
                                if (lastRawValue != null)
                                {
                                    // update the expression's value
                                    var computeExpressionService = _scope.ServiceProvider
                                        .GetRequiredService<IComputeExpressionService>();
                                    
                                    computeExpressionService.UpdateValue(exp.Guid, lastRawValue.Value);
                                }
                                else
                                {
                                    // TODO: mark as failed
                                }
                                
                                break;
                            case ComputeExpressionType.Computed:
                                var computeValueEvent = _scope.ServiceProvider.GetRequiredService<IComputeValueEvent>();

                                var lastValue = computeValueEvent.GetLastItem(exp.Expression);
                                if (lastValue != null)
                                {
                                    // update the expression's value
                                    var computeExpressionService = _scope.ServiceProvider
                                        .GetRequiredService<IComputeExpressionService>();
                                    
                                    computeExpressionService.UpdateValue(exp.Guid, lastValue.Value);
                                }
                                else
                                {
                                    // TODO: mark as failed
                                }
                                
                                break;
                            case ComputeExpressionType.Analysed:
                                var analysedComponentEvent = _scope.ServiceProvider
                                    .GetRequiredService<IAnalysedComponentEvent>();

                                var lastAnalysedValue = analysedComponentEvent.Get(exp.Expression);
                                if (lastAnalysedValue != null)
                                {
                                    var computeExpressionService = _scope.ServiceProvider
                                        .GetRequiredService<IComputeExpressionService>();
                                    
                                    computeExpressionService.UpdateValue(exp.Guid, lastAnalysedValue.Value);
                                }
                                else
                                {
                                    // TODO: mark as failed
                                }
                                
                                break;
                            default:
                                _logger.LogInformation($"{_computeServiceName} ExecuteAsync: Ignoring " +
                                                       $"expression {exp.Guid} for updating due to its type.");
                                break;
                        }
                    }
                }
            }

            _logger.LogCritical($"{_computeServiceName} ExecuteAsync: Shutting down!");
            return Task.CompletedTask;
        }
    }
}