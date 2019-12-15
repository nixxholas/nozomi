using System.Data;
using FluentValidation;

namespace Nozomi.Data.ViewModels.Currency
{
    public class ModifyCurrencyViewModel : CurrencyViewModel
    {
        public long? Id { get; set; }
        
        public bool IsEnabled { get; set; }

        public new bool IsValid()
        {
            var validator = new ModifyCurrencyValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class ModifyCurrencyValidator : AbstractValidator<ModifyCurrencyViewModel>
        {
            public ModifyCurrencyValidator()
            {
                RuleFor(e => e.Id)
                    .GreaterThan(0)
                    .Unless(e => !string.IsNullOrWhiteSpace(e.Slug) 
                                 && !string.IsNullOrEmpty(e.Slug));
                RuleFor(e => e.IsEnabled).NotNull();
                RuleFor(e => e.CurrencyTypeGuid).NotNull().NotEmpty();
                RuleFor(e => e.Abbreviation).NotNull().NotEmpty();
                RuleFor(e => e.Slug).NotNull().NotEmpty()
                    .Unless(e => e.Id != null && e.Id > 0);
                RuleFor(e => e.Name).NotNull().NotEmpty();
                RuleFor(e => e.LogoPath);
                RuleFor(e => e.Description);
                RuleFor(e => e.Denominations).GreaterThanOrEqualTo(0);
                RuleFor(e => e.DenominationName);
            }
        }
    }
}