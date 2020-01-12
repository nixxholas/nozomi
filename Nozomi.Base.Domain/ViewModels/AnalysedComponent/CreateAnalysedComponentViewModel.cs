using System;
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
        
        public string CurrencySlug { get; set; }
        
        public string CurrencyPairGuid { get; set; }
        
        public string CurrencyTypeShortForm { get; set; }

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
                RuleFor(e => e.CurrencySlug).NotNull()
                    .Unless(e => Guid.TryParse(e.CurrencyPairGuid, out var cpGuid)
                                 || !string.IsNullOrEmpty(e.CurrencyTypeShortForm));
                RuleFor(e => e.CurrencyPairGuid)
                    .Must(e => Guid.TryParse(e, out var cpGuid))
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug)
                                 || !string.IsNullOrEmpty(e.CurrencyTypeShortForm));
                RuleFor(e => e.CurrencyTypeShortForm)
                    .Must(e => !string.IsNullOrEmpty(e) && !string.IsNullOrWhiteSpace(e))
                    .Unless(e => !string.IsNullOrEmpty(e.CurrencySlug)
                                 || Guid.TryParse(e.CurrencyPairGuid, out var cpGuid));
            }
        }
    }
}