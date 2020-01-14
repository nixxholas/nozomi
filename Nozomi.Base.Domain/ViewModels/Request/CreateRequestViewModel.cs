using FluentValidation;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.Request
{
    public class CreateRequestViewModel
    {
        /// <summary>
        /// Request Type. GET? PUT?
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// JSON? XML?
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// URL to the endpoint
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// The delay between each poll.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// The delay after a failed poll attempt
        /// </summary>
        public long FailureDelay { get; set; }

        /// <summary>
        /// This will deduce what type of request this is for
        /// i.e. CurrencyType, CurrencyPair or Currency.
        /// </summary>
        public RequestParentType ParentType { get; set; }

        public enum RequestParentType
        {
            Currency = 0,
            CurrencyPair = 1,
            CurrencyType = 2
        }

        /// <summary>
        /// The unique abbreviation of a currency.
        /// </summary>
        public string CurrencySlug { get; set; }

        /// <summary>
        /// The distinct guid of the currency pair selected.
        /// </summary>
        public string CurrencyPairGuid { get; set; }

        /// <summary>
        /// The Guid of the Currency Type selected.
        /// </summary>
        public string CurrencyTypeGuid { get; set; }

        public bool IsValid()
        {
            var validator = new CreateRequestValidator();
            return validator.Validate(this).IsValid;
        }

        protected class CreateRequestValidator : AbstractValidator<CreateRequestViewModel>
        {
            public CreateRequestValidator()
            {
                RuleFor(r => r.RequestType).IsInEnum();
                RuleFor(r => r.ResponseType).IsInEnum();
                RuleFor(r => r.DataPath).NotEmpty();
                RuleFor(r => r.Delay).GreaterThan(-1);
                RuleFor(r => r.FailureDelay).GreaterThan(-1);
                RuleFor(r => r.ParentType).IsInEnum();
                // Safetynet for Currencies
                RuleFor(r => r.CurrencySlug).NotNull().NotEmpty()
                    // Ignore the check if a currency pair or currency type is selected.
                    .Unless(r => 
                        !string.IsNullOrEmpty(r.CurrencyPairGuid) || !string.IsNullOrEmpty(r.CurrencyTypeGuid));
                // Safetynet for Currency Pairs
                RuleFor(r => r.CurrencyPairGuid).NotEmpty().NotNull()
                    // Ignore the check if a currency or currency type is selected.
                    .Unless(r => 
                        !string.IsNullOrEmpty(r.CurrencyTypeGuid) || !string.IsNullOrEmpty(r.CurrencySlug));
                // Safetynet for Currency types
                RuleFor(r => r.CurrencyTypeGuid).NotEmpty().NotNull()
                    .Unless(r =>
                        // Ignore the check if a currency or currency pair is selected
                        !string.IsNullOrEmpty(r.CurrencySlug) || !string.IsNullOrEmpty(r.CurrencyPairGuid));
            }
        }
    }
}