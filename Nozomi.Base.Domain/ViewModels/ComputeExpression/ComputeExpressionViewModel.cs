using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.ComputeExpression
{
    public class ComputeExpressionViewModel : CreateComputeExpressionViewModel
    {
        public Guid Guid { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public Models.Web.Compute Compute { get; set; }

        public ComputeExpressionViewModel()
        {
        }

        public ComputeExpressionViewModel(Guid guid, ComputeExpressionType type, string expression, string value,
            Guid computeGuid, bool isEnabled)
        {
            Guid = guid;
            Type = type;
            Expression = expression;
            Value = value;
            ComputeGuid = computeGuid;
            IsEnabled = isEnabled;
        }
        
        public ComputeExpressionViewModel(Guid guid, ComputeExpressionType type, string expression, string value,
            Guid computeGuid, bool isEnabled, Models.Web.Compute compute)
        {
            Guid = guid;
            Type = type;
            Expression = expression;
            Value = value;
            ComputeGuid = computeGuid;
            IsEnabled = isEnabled;

            Compute = compute;
        }
    }
}