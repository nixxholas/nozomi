using FluentValidation;

namespace Nozomi.Data.ViewModels.CurrencyPair
{
    public class UpdateCurrencyPairViewModel : CurrencyPairViewModel
    {
        public bool IsValid()
        {
            var validator = new UpdateCurrencyPairValidator();
            return validator.Validate(this).IsValid;
        }
        
        public class UpdateCurrencyPairValidator : AbstractValidator<UpdateCurrencyPairViewModel>
        {
            public UpdateCurrencyPairValidator()
            {
                RuleFor(e => e.Guid).NotNull().NotEmpty();
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