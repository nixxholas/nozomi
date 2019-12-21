using System;
using FluentValidation;

namespace Nozomi.Data.ViewModels.Currency
{
    public class CreateCurrencyViewModel : BaseCurrencyViewModel
    {
        public Guid CurrencyTypeGuid { get; set; }
        
        public string LogoPath { get; set; }
        
        public string Description { get; set; }

        public int Denominations { get; set; } = 0;
        
        public string DenominationName { get; set; }

        public bool IsValid()
        {
            var validator = new CreateCurrencyValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyViewModel>
        {
            public CreateCurrencyValidator()
            {
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