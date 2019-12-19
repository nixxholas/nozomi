using FluentValidation;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ViewModels.CurrencyPair
{
    public class CreateCurrencyPairViewModel
    {
        public CurrencyPairType Type { get; set; }
        
        public string ApiUrl { get; set; }
        
        public string DefaultComponent { get; set; }
        
        public string SourceGuid { get; set; }

        public string MainTicker { get; set; }
        
        public string CounterTicker { get; set; }
        
        public bool IsEnabled { get; set; }

        public bool IsValid()
        {
            var validator = new CreateCurrencyPairValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class CreateCurrencyPairValidator : AbstractValidator<CreateCurrencyPairViewModel>
        {
            public CreateCurrencyPairValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                // RuleFor(e => e.ApiUrl);
                // RuleFor(e => e.DefaultComponent);
                RuleFor(e => e.SourceGuid).NotEmpty().NotNull();
                RuleFor(e => e.MainTicker).NotNull().NotEmpty();
                RuleFor(e => e.CounterTicker).NotNull().NotEmpty();
            }
        }
    }
}