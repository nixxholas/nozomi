using System.ComponentModel;
using FluentValidation;

namespace Nozomi.Data.ViewModels.CurrencyType
{
    public class UpdateCurrencyTypeViewModel : CurrencyTypeViewModel
    {
        [DefaultValue(false)]
        public bool Delete { get; set; }

        public bool IsValid()
        {
            var validator = new UpdateCurrencyTypeValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class UpdateCurrencyTypeValidator : AbstractValidator<UpdateCurrencyTypeViewModel>
        {
            public UpdateCurrencyTypeValidator()
            {
                RuleFor(st => st.TypeShortForm)
                    .Must(e => e.Length <= 12);
            }
        }
    }
}