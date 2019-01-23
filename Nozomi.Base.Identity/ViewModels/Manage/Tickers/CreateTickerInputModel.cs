using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Base.Identity.ViewModels.Manage.Tickers
{
    public class CreateTickerInputModel
    {
        public IEnumerable<CurrencyPairType> CurrencyPairTypes { get; }
            = Enum.GetValues(typeof(CurrencyPairType)).Cast<CurrencyPairType>();

        [Required]
        [DisplayName("The type of this currency pair.")]
        public CurrencyPairType CurrencyPairType { get; set; } = CurrencyPairType.UNKNOWN;
        
        public IEnumerable<RequestType> RequestTypes { get; }
            = Enum.GetValues(typeof(RequestType)).Cast<RequestType>();

        [Required]
        [DisplayName("Defines the request type of the request.")]
        public RequestType RequestType { get; set; }

        public IEnumerable<ResponseType> ResponseTypes { get; }
            = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>();

        [Required]
        [DisplayName("Defines the response type of the request.")]
        public ResponseType ResponseType { get; set; }
        
        [Url]
        [Required]
        [DisplayName("Defines the URL of the API.")]
        public string DataPath { get; set; }

        [Required]
        [DisplayName("Defines the polling frequency in milliseconds.")]
        public int Delay { get; set; } = 5000;
        
        [Required]
        [DisplayName("The entity this API is provided from.")]
        public long CurrencySourceId { get; set; }
        
        [Required]
        [DisplayName("The main currency of the pair.")]
        public long MainCurrencyId { get; set; }
        
        [Required]
        [DisplayName("The sub currency of the pair.")]
        public long CounterCurrencyId { get; set; }
        
        [Required]
        [DisplayName("The component variables that we'll be querying from the response.")]
        public string QueryComponents { get; set; }
    }
}