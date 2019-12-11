using FluentValidation;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ResponseModels.CurrencyPair;

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
        public int FailureDelay { get; set; }

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
        /// The distinct response of the currency pair selected.
        /// </summary>
        /// TODO: Rewrite
        public DistinctCurrencyPairResponse CurrencyPair { get; set; }

        /// <summary>
        /// The ID of the Currency Type selected.
        /// </summary>
        public long CurrencyTypeId { get; set; }

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

                RuleFor(r => r.CurrencySlug).NotNull().Unless(r => r.CurrencyPair != null
                                                                   || r.CurrencyTypeId > 0);
                RuleFor(r => r.CurrencyPair).NotNull().Unless(r =>
                    !string.IsNullOrEmpty(r.CurrencySlug) || r.CurrencyTypeId > 0);
                RuleFor(r => r.CurrencyPair.Id).Must(v => v > 0).Unless(r =>
                    !string.IsNullOrEmpty(r.CurrencySlug) || r.CurrencyTypeId > 0);
                RuleFor(r => r.CurrencyTypeId).GreaterThan(0).Unless(r =>
                    !string.IsNullOrEmpty(r.CurrencySlug) || r.CurrencyPair != null);
            }
        }
    }
}
