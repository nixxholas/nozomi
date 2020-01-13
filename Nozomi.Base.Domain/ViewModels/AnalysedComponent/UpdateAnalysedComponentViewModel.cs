using FluentValidation;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class UpdateAnalysedComponentViewModel : AnalysedComponentViewModel
    {
        public new bool IsValid()
        {
            var validator = new UpdateAnalysedComponentValidator();
            return validator.Validate(this).IsValid;
        }

        protected class UpdateAnalysedComponentValidator : AbstractValidator<UpdateAnalysedComponentViewModel>
        {
            public UpdateAnalysedComponentValidator()
            {
                RuleFor(e => e.Guid).NotEmpty().NotNull();
                RuleFor(e => e.Type).IsInEnum();
                RuleFor(e => e.Delay).GreaterThanOrEqualTo(0);
                // RuleFor(e => e.UiFormatting); // No rules yet
                RuleFor(e => e.IsDenominated).NotNull();
                RuleFor(e => e.StoreHistoricals).NotNull();
                RuleFor(e => e.CurrencySlug).NotNull().NotEmpty()
                    .Unless(e => 
                        System.Guid.TryParse(e.CurrencyPairGuid, out var cpGuid)
                                 || !string.IsNullOrEmpty(e.CurrencyTypeShortForm));
                RuleFor(e => e.CurrencyPairGuid)
                    .Must(e => System.Guid.TryParse(e, out var cpGuid))
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug)
                                 || !string.IsNullOrEmpty(e.CurrencyTypeShortForm));
                RuleFor(e => e.CurrencyTypeShortForm)
                    .Must(e => !string.IsNullOrEmpty(e) && !string.IsNullOrWhiteSpace(e))
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug)
                                 || System.Guid.TryParse(e.CurrencyPairGuid, out var cpGuid));
            }
        }
    }
}