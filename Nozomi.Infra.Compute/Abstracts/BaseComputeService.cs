using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCalc;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Compute.Abstracts
{
    public abstract class BaseComputeService<T> : BaseHostedService<T> where T : class
    {
        public readonly string _computeServiceName;
        
        protected BaseComputeService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            _computeServiceName = typeof(T).FullName;
        }

        /// <summary>
        /// Executes the computation via the Expression engine.
        /// </summary>
        /// <param name="compute">The most parent compute.</param>
        protected void ExecuteComputation(Data.Models.Web.Compute compute)
        {
            if (compute == null)
                return;
            
            _logger.LogInformation($"{_computeServiceName} ExecuteComputation: Updating computation for " +
                                   $"{compute.Guid}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var computeService = scope.ServiceProvider.GetRequiredService<IComputeService>();

                // Look for any expression without a value first
                if (compute.Expressions.Any(e => string.IsNullOrEmpty(e.Value)))
                {
                    _logger.LogInformation($"{_computeServiceName} ExecuteComputation: An expression in compute " +
                                           $"{compute.Guid} has no value.");
                    computeService.Modified(compute.Guid);
                    return;
                }

                // Then we prepare to compute.
                var expression = new Expression(compute.Formula);
                foreach (var expVal in compute.Expressions)
                {
                    // If there is a parameter in the expression
                    if (expression.Parameters.Any(p => p.Key.Equals(expVal.Expression)))
                    {
                        _logger.LogWarning($"{_computeServiceName} ExecuteComputation: An expression in " +
                                           $"{compute.Guid} has a duplicate expression: {expVal.Expression}");
                        expression.Parameters[expVal.Expression] = expVal.Value; // Replace it
                    }
                    else
                    {
                        expression.Parameters.Add(expVal.Expression, expVal.Value); // Add it in
                    }
                }

                // Any children? And ensure there are missing expressions too.
                if (compute.ChildComputes.Any())
                {
                    // Look for any children without compute value next.
                    if (compute.ChildComputes.Any(cc =>
                        cc.ChildCompute.Values.OrderByDescending(v => v.CreatedAt).FirstOrDefault()
                            ?.Value == null))
                    {
                        _logger.LogInformation($"{_computeServiceName} ExecuteComputation: A child Compute has no " +
                                               "value.");
                        computeService.Modified(compute.Guid);
                        return;
                    }

                    // Populate the expression further
                    foreach (var childCompute in compute.ChildComputes)
                    {
                        // Obtain the latest child compute value
                        var latestChildValue = childCompute.ChildCompute.Values
                            .OrderByDescending(v => v.CreatedAt)
                            .FirstOrDefault();

                        // Ensure we check against the parameters in the expression as well
                        if (!expression.Parameters.Any(p => p.Key
                            .Equals(childCompute.ChildComputeGuid.ToString())))
                            _logger.LogWarning($"{_computeServiceName} ExecuteComputation: compute {compute.Guid}" +
                                               " has a duplicate child compute for expression " +
                                               $"{childCompute.ChildCompute.Guid}");

                        // Pump it in
                        if (latestChildValue != null && !string.IsNullOrEmpty(latestChildValue.Value))
                            expression.Parameters[childCompute.ChildComputeGuid.ToString()] =
                                latestChildValue.Value; // Set
                    }
                }

                try
                {
                    // Nope, let's compute
                    var evaluatedExpression = expression.Evaluate();
                    if (evaluatedExpression != null)
                    {
                        var computeValueService = scope.ServiceProvider.GetRequiredService<IComputeValueService>();

                        var newComputeValue = new ComputeValue
                        {
                            Value = evaluatedExpression.ToString(),
                            ComputeGuid = compute.Guid
                        };

                        computeValueService.Push(newComputeValue);
                        computeService.Modified(compute.Guid);
                        _logger.LogInformation($"{_computeServiceName} ExecuteCompuation: Computation {compute.Guid}" +
                                               " successfully updated!");
                        return;
                    }
                }
                catch (ArgumentException argumentException)
                {
                    _logger.LogWarning($"{_computeServiceName} ExecuteComputation/Evaluation: compute " +
                                       $"{compute.Guid} => {argumentException.Message}");
                }

                computeService.Modified(compute.Guid, true); // failed
            }
        }
    }
}