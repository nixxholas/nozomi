using System;
using FluentValidation;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class CreateAnalysedComponentViewModel
    {
        /// <summary>
        /// The type of this analysed component.
        /// </summary>
        public AnalysedComponentType Type { get; set; }

        /// <summary>
        /// Delay for every value update attempt in milliseconds.
        /// </summary>
        public int Delay { get; set; }
        
        /// <summary>
        /// The UI formatting for the value via numeral.js' syntax.
        /// </summary>
        public string UiFormatting { get; set; }
        
        /// <summary>
        /// Denotes if there's denomination for its value.
        /// </summary>
        public bool IsDenominated { get; set; }
        
        /// <summary>
        /// Store the historical data for this?
        /// </summary>
        public bool StoreHistoricals { get; set; }
        
        /// <summary>
        /// The symlinking currency you are referring to; through its slug.
        /// </summary>
        public string CurrencySlug { get; set; }
        
        /// <summary>
        /// The symlinking currency pair you are referring to; through its guid.
        /// </summary>
        public string CurrencyPairGuid { get; set; }
        
        /// <summary>
        /// The symlinking currency type youa re referring to; through its short form.
        /// </summary>
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