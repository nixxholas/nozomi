using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.Compute
{
    public class CreateComputeViewModel
    {
        public string Key { get; set; }
                
        public string Formula { get; set; }
               
        public int Delay { get; set; }
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