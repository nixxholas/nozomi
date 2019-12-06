using FluentValidation;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class CreateAnalysedComponentViewModel
    {
        public AnalysedComponentType Type { get; set; }
        
        public int Delay { get; set; }
        
        public string UiFormatting { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public long CurrencyId { get; set; }
        
        public string CurrencySlug { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public long CurrencyTypeId { get; set; }

        public bool IsValid()
        {
            var validator = new CreateAnalysedComponentValidator();
            return validator.Validate(this).IsValid;
        }
        
        protected class CreateAnalysedComponentValidator : AbstractValidator<CreateAnalysedComponentViewModel>
        {
            public CreateAnalysedComponentValidator()
            {
                RuleFor(e => e.Type).IsInEnum();
                RuleFor(e => e.Delay).GreaterThanOrEqualTo(0);
                // RuleFor(e => e.UiFormatting); // No rules yet
                RuleFor(e => e.IsDenominated).NotNull();
                RuleFor(e => e.StoreHistoricals).NotNull();
                RuleFor(e => e.CurrencyId).GreaterThan(0)
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug) || e.CurrencyPairId > 0 || e.CurrencyTypeId > 0);
                RuleFor(e => e.CurrencySlug).NotNull()
                    .Unless(e => e.CurrencyId > 0 || e.CurrencyPairId > 0 || e.CurrencyTypeId > 0);
                RuleFor(e => e.CurrencyPairId).GreaterThan(0)
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug) || e.CurrencyId > 0 || e.CurrencyTypeId > 0);
                RuleFor(e => e.CurrencyTypeId).GreaterThan(0)
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug) || e.CurrencyId > 0 || e.CurrencyPairId > 0);
            }
        }
    }
}