using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.ComputeValue
{
    public class UpdateComputeValueViewModel : ComputeValueViewModel
    {
        public new bool? IsEnabled { get; set; }

        public new Guid? ComputeGuid { get; set; }
        
        public new string Value { get; set; }

        protected class UpdateComputeValueViewModelValidator : AbstractValidator<UpdateComputeValueViewModel>
        {
            public UpdateComputeValueViewModelValidator()
            {
                RuleFor(e => e.Guid).NotNull().NotEmpty();
                RuleFor(e => e.ComputeGuid).NotNull().NotEmpty();
                RuleFor(e => e.Value).NotNull().NotEmpty();
            }
        }
    }
}