using System.Data;
using FluentValidation;

namespace Nozomi.Data.ViewModels.Currency
{
    public class ModifyCurrencyViewModel : CurrencyViewModel
    {
        public long Id { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public class ModifyCurrencyValidator : AbstractValidator<ModifyCurrencyViewModel>
        {
            public ModifyCurrencyValidator()
            {
                RuleFor(e => e.Id).GreaterThan(0);
                RuleFor(e => e.IsEnabled).NotNull();
                RuleFor(e => e.CurrencyTypeGuid).NotNull();
                RuleFor(e => e.Abbreviation).NotNull().NotEmpty();
                RuleFor(e => e.Slug).NotNull().NotEmpty();
                RuleFor(e => e.Name).NotNull().NotEmpty();
                RuleFor(e => e.LogoPath);
                RuleFor(e => e.Description);
                RuleFor(e => e.Denominations).GreaterThanOrEqualTo(0);
                RuleFor(e => e.DenominationName);
            }
        }
    }
}