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
        public ComputeExpressionHostedService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var computeExpressionEvent =
                            scope.ServiceProvider.GetRequiredService<IComputeExpressionEvent>();

                        var mostOutdatedExps = computeExpressionEvent.GetByAge()
                            .Where(e => !e.Type.Equals(ComputeExpressionType.Generic))
                            .ToList();

                        if (mostOutdatedExps.Any())
                        {
                            foreach (var exp in mostOutdatedExps)
                            {
                                var computeExpressionService = scope.ServiceProvider
                                    .GetRequiredService<IComputeExpressionService>();

                                switch (exp.Type)
                                {
                                    case ComputeExpressionType.Raw:
                                        var componentHistoricItemEvent = scope.ServiceProvider
                                            .GetRequiredService<IComponentHistoricItemEvent>();

                                        var lastRawValue = componentHistoricItemEvent
                                            .GetLastItem(exp.Expression);
                                        if (lastRawValue != null)
                                        {
                                            // update the expression's value
                                            computeExpressionService.UpdateValue(exp.Guid, lastRawValue.Value);
                                        }
                                        else
                                        {
                                            computeExpressionService.UpdateValue(exp.Guid, null);
                                        }

                                        break;
                                    case ComputeExpressionType.Computed:
                                        var computeValueEvent =
                                            scope.ServiceProvider.GetRequiredService<IComputeValueEvent>();

                                        var lastValue = computeValueEvent.GetLastItem(exp.Expression);
                                        if (lastValue != null)
                                        {
                                            // update the expression's value
                                            computeExpressionService.UpdateValue(exp.Guid, lastValue.Value);
                                        }
                                        else
                                        {
                                            computeExpressionService.UpdateValue(exp.Guid, null);
                                        }

                                        break;
                                    case ComputeExpressionType.Analysed:
                                        var analysedComponentEvent = scope.ServiceProvider
                                            .GetRequiredService<IAnalysedComponentEvent>();

                                        var lastAnalysedValue = analysedComponentEvent.Get(exp.Expression);
                                        if (lastAnalysedValue != null)
                                        {
                                            computeExpressionService.UpdateValue(exp.Guid, lastAnalysedValue.Value);
                                        }
                                        else
                                        {
                                            computeExpressionService.UpdateValue(exp.Guid, null);
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

                    await Task.Delay(10, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_computeServiceName} ExecuteAsync: {ex.Message}");
            }

            _logger.LogCritical($"{_computeServiceName} ExecuteAsync: Shutting down!");
        }
    }
}