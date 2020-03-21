using System;
using FluentValidation;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.ComputeExpression
{
    public class UpdateComputeExpressionViewModel : ComputeExpressionViewModel
    {
        public new bool? IsEnabled { get; set; }

        protected class UpdateComputeExpressionViewModelValidator : AbstractValidator<UpdateComputeExpressionViewModel>
        {
            public UpdateComputeExpressionViewModelValidator()
            {
                RuleFor(e => e.Guid).NotNull().NotEmpty();

                RuleFor(e => e.Type).IsInEnum();
                RuleFor(e => e.Expression).NotNull().NotEmpty();
                RuleFor(e => e.Value).NotNull().NotEmpty();
                RuleFor(e => e.ComputeGuid).NotNull().NotEmpty();
            }
        }
    }
}