using System;
using FluentValidation;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.ComputeValue;

namespace Nozomi.Data.ViewModels.ComputeExpression
{
    public class CreateComputeExpressionViewModel
    {
        public ComputeExpressionType Type { get; set; }
        
        public string Expression { get; set; }
        
        public string Value { get; set; }
        
        public Guid ComputeGuid { get; set; }

        public bool IsValid()
        {
            var validator = new CreateComputeExpressioNViewModelValidator();
            return validator.Validate(this).IsValid;
        }

        protected class CreateComputeExpressioNViewModelValidator : AbstractValidator<CreateComputeExpressionViewModel> {
            public CreateComputeExpressioNViewModelValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                RuleFor(e => e.Expression).NotNull().NotEmpty();
                RuleFor(e => e.Value).NotNull().NotEmpty();
                RuleFor(e => e.ComputeGuid).NotNull().NotEmpty();
            }
        }
    }
}