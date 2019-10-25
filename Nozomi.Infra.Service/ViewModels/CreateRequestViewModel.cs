using Nozomi.Data.Models;
using Nozomi.Data.ResponseModels.CurrencyPair;

namespace Nozomi.Service.ViewModels
{
    public class CreateRequestViewModel
    {
        /// <summary>
        /// Request Type. GET? PUT?
        /// </summary>
        public RequestType Type { get; set; }

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
        public DistinctCurrencyPairResponse CurrencyPair { get; set; }

        /// <summary>
        /// The ID of the Currency Type selected.
        /// </summary>
        public long CurrencyTypeId { get; set; }
    }
}
