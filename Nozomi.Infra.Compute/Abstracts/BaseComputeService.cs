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
        
        protected BaseComputeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _computeServiceName = typeof(T).FullName;
        }

        /// <summary>
        /// Executes the computation via the Expression engine.
        /// </summary>
        /// <param name="compute">The most parent compute.</param>
        protected void ExecuteComputation(Data.Models.Web.Compute compute)
        {
            // Look for any expression without a value first
            if (compute.Expressions.Any(e => string.IsNullOrEmpty(e.Value)))
            {
                _logger.LogInformation($"{_computeServiceName} ExecuteComputation: An expression in compute " +
                                       $"{compute.Guid} has no value.");
                return;
            }
            
            // Then we prepare to compute.
            var expression = new Expression(compute.Formula);
            foreach (var expVal in compute.Expressions)
            {
                // If there is a parameter in the expression
                if (expression.Parameters.Any(p => p.Key.Equals(expVal.Expression)))
                {
                    expression.Parameters[expVal.Expression] = expVal.Value; // Add it in
                }
                else
                {
                    // Warn since this doesn't exist
                    _logger.LogWarning($"{_computeServiceName} ExecuteComputation: Expression {expVal.Guid}" +
                                       $" has no presence in compute {compute.Guid}.");
                }
            }
            
            // Are there any expressions missed out?
            var missingExpressions = expression.Parameters.Any(p => p.Value == null);
                
            // Any children? And ensure there are missing expressions too.
            if (compute.ChildComputes.Any() && missingExpressions)
            {
                // Look for any children without compute value next.
                if (compute.ChildComputes.Any(cc =>
                    cc.ChildCompute.Values.OrderByDescending(v => v.CreatedAt).FirstOrDefault()
                        ?.Value == null))
                {
                    _logger.LogInformation($"{_computeServiceName} ExecuteComputation: A child Compute has no " +
                                           "value.");
                    return;
                }
                else
                {
                    // Populate the expression further
                    foreach (var childCompute in compute.ChildComputes)
                    {
                        // Obtain the latest child compute value
                        var latestChildValue = childCompute.ChildCompute.Values
                            .OrderByDescending(v => v.CreatedAt)
                            .FirstOrDefault();

                        // Pump it in
                        if (latestChildValue != null && !string.IsNullOrEmpty(latestChildValue.Value)
                                                     // Ensure we check against the parameters in the expression as well
                        && expression.Parameters
                            .Any(p => p.Key.Equals(childCompute.ChildComputeGuid.ToString())))
                        {
                            // Set
                            expression.Parameters[childCompute.ChildComputeGuid.ToString()] = latestChildValue.Value;
                        }
                    }
                }
            }
            else if (!compute.ChildComputes.Any() && missingExpressions)
            {
                _logger.LogWarning($"{_computeServiceName} ExecuteComputation: Expression for compute " +
                                   $"{compute.Guid} has an null parameter value.");
                return;
            }
            
            // Nope, let's compute
            var evaluatedExpression = expression.Evaluate();
            if (evaluatedExpression != null)
            {
                var computeValueService = _scope.ServiceProvider.GetRequiredService<IComputeValueService>();
                
                var newComputeValue = new ComputeValue
                {
                    Value = evaluatedExpression.ToString(),
                    ComputeGuid = compute.Guid
                };
                
                computeValueService.Push(newComputeValue);
            }
        }
    }
}