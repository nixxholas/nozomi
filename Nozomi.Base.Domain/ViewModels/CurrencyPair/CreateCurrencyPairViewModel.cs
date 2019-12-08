using FluentValidation;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ViewModels.CurrencyPair
{
    public class CreateCurrencyPairViewModel
    {
        public CurrencyPairType CurrencyPairType { get; set; }
        
        public string ApiUrl { get; set; }
        
        public string DefaultComponent { get; set; }
        
        public long SourceId { get; set; }
        
        public string MainCurrencyAbbrv{ get; set; }
        
        public string CounterCurrencyAbbrv { get; set; }
        
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
                RuleFor(e => e.CurrencyPairType).IsInEnum();
                // RuleFor(e => e.ApiUrl);
                // RuleFor(e => e.DefaultComponent);
                RuleFor(e => e.SourceId).GreaterThan(0);
                RuleFor(e => e.MainCurrencyAbbrv).NotNull().NotEmpty();
                RuleFor(e => e.CounterCurrencyAbbrv).NotNull().NotEmpty();
            }
        }
    }
}