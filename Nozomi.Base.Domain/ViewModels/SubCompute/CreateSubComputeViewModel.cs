using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.SubCompute
{
    public class CreateSubComputeViewModel
    {
        public Guid ParentComputeGuid { get; set; }
        public Guid ChildComputeGuid { get; set; }

        public bool IsValid()
        {
            var validator = new CreateSubComputeViewModelValidator();
            return validator.Validate(this).IsValid;
        }

        protected class CreateSubComputeViewModelValidator : AbstractValidator<CreateSubComputeViewModel>
        {
            public CreateSubComputeViewModelValidator()
            {
                RuleFor(e => e.ParentComputeGuid).NotNull().NotEmpty();
            }
        }
    }
}