using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.Compute
{
    public class CreateComputeViewModel : BaseComputeViewModel
    {
        
    }

    public class CreateComputeViewModelValidator : AbstractValidator<CreateComputeViewModel>
    {
        public CreateComputeViewModelValidator()
        {
            RuleFor(e => e.Key).NotNull();
            RuleFor(e => e.Formula).NotNull().NotEmpty();
            RuleFor(e => e.Delay).NotNull().NotEmpty();
        }
    }
}