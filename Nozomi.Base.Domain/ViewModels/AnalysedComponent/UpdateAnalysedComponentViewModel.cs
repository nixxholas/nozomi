using FluentValidation;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class UpdateAnalysedComponentViewModel : AnalysedComponentViewModel
    {
        public bool IsValid()
        {
            var validator = new UpdateAnalysedComponentValidator();
            return validator.Validate(this).IsValid;
        }
        
        protected class UpdateAnalysedComponentValidator : AbstractValidator<UpdateAnalysedComponentViewModel>
        {
            public UpdateAnalysedComponentValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                RuleFor(e => e.Delay).GreaterThanOrEqualTo(0);
                // RuleFor(e => e.UiFormatting); // No rules yet
                RuleFor(e => e.IsDenominated).NotNull();
                RuleFor(e => e.StoreHistoricals).NotNull();
                RuleFor(e => e.CurrencyId).GreaterThan(0)
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug) 
                                 || System.Guid.TryParse(e.CurrencyPairId, out var cpGuid) 
                                 || System.Guid.TryParse(e.CurrencyTypeId, out var ctGuid));
                RuleFor(e => e.CurrencySlug).NotNull()
                    .Unless(e => e.CurrencyId > 0 
                                 || System.Guid.TryParse(e.CurrencyPairId, out var cpGuid) 
                                 || System.Guid.TryParse(e.CurrencyTypeId, out var ctGuid));
                RuleFor(e => e.CurrencyPairId).NotNull().NotEmpty()
                    .Must(e => System.Guid.TryParse(e, out var cpGuid))
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug) 
                                 || e.CurrencyId > 0 || System.Guid.TryParse(e.CurrencyTypeId, out var ctGuid));
                RuleFor(e => e.CurrencyTypeId).NotNull().NotEmpty()
                    .Must(e => System.Guid.TryParse(e, out var ctGuid))
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug) 
                                 || e.CurrencyId > 0 || System.Guid.TryParse(e.CurrencyPairId, out var cpGuid));
            }
        }
    }
}