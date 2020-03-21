using System;
using System.Collections.Generic;
using FluentValidation;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.Compute
{
    public class UpdateComputeViewModel : ComputeViewModel
    {
        public new bool? IsEnabled { get; set; }
        public new string Key { get; set; }
        public new string Formula { get; set; }
        public new int? Delay { get; set; }

        public new bool IsValid()
        {
            var validator = new UpdateComputeValidator();
            return validator.Validate(this).IsValid;
        }

        protected class UpdateComputeValidator : AbstractValidator<UpdateComputeViewModel>
        {
            public UpdateComputeValidator()
            {
                RuleFor(e => e.Guid).NotEmpty().NotNull();   
                RuleFor(e => e.Key).NotNull();
                RuleFor(e => e.Formula).NotNull().NotEmpty();
                RuleFor(e => e.Delay).NotNull().NotEmpty();
            }
        }
    }
}