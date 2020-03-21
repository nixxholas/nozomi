using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.SubCompute
{
    public class UpdateSubComputeViewModel : SubComputeViewModel
    {
        public new bool? IsEnabled { get; set; }

        protected class UpdateSubComputeViewModelValidator : AbstractValidator<UpdateSubComputeViewModel>
        {
            public UpdateSubComputeViewModelValidator()
            {
                RuleFor(e => e.Guid).NotNull().NotEmpty();

                RuleFor(e => e.ParentComputeGuid).NotNull().NotEmpty();
            }
        }
    }
}