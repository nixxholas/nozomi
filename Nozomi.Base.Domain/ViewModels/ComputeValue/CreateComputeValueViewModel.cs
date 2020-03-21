using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.ComputeValue
{
    public class CreateComputeValueViewModel
    {
        public string Value { get; set; }
        
        public Guid ComputeGuid { get; set; }

        public bool IsValid()
        {
            var validator = new CreateComputeValueViewModelValidator();
            return validator.Validate(this).IsValid;
        }

        protected class CreateComputeValueViewModelValidator : AbstractValidator<CreateComputeValueViewModel>
        {
            public CreateComputeValueViewModelValidator()
            {
                RuleFor(e => e.Value).NotNull().NotEmpty();
                RuleFor(e => e.ComputeGuid).NotNull().NotEmpty();
            }
        }
    }
}