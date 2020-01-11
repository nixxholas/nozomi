using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.SourceType
{
    public class UpdateSourceTypeViewModel : SourceTypeViewModel
    {
        public bool? IsEnabled { get; set; }
        
        public DateTime? DeletedAt { get; set; }

        public bool IsValid()
        {
            var validator = new UpdateSourceTypeValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class UpdateSourceTypeValidator : AbstractValidator<UpdateSourceTypeViewModel>
        {
            public UpdateSourceTypeValidator()
            {
                RuleFor(e => e.Guid).NotNull().NotEmpty();
            }
        }
    }
}