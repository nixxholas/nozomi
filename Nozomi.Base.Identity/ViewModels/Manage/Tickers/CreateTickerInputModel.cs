using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nozomi.Base.Core;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Base.Identity.ViewModels.Manage.Tickers
{
    public class CreateTickerInputModel : BaseViewModel
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
        [DisplayName("Frequency (In milliseconds)")]
        public int Delay { get; set; } = 5000;
        
        [Required]
        [DisplayName("The entity this API is provided from.")]
        public long CurrencySourceId { get; set; }
        
        [Required]
        [DisplayName("The type of the main currency.")]
        public long MainCurrencyTypeId { get; set; }
        
        [Required]
        [DisplayName("The abbreviation of the main currency.")]
        public string MainCurrencyAbbrv { get; set; }
        
        [DisplayName("The name of the main currency. (Optional if a ticker from this source already has this currency)")]
        public string MainCurrencyName { get; set; }
        
        [Required]
        [DisplayName("The type of the counter currency.")]
        public long CounterCurrencyTypeId { get; set; }
        
        [Required]
        [DisplayName("The abbreviation of the counter currency.")]
        public string CounterCurrencyAbbrv { get; set; }
        
        [DisplayName("The name of the counter currency. (Optional if a ticker from this source already has this currency)")]
        public string CounterCurrencyName { get; set; }
        
        [Required]
        [DisplayName("The component variables that we'll be querying from the response.")]
        public string QueryComponents { get; set; }
        
        [DisplayName("The properties of the request. (Could be a header property)")]
        public string RequestProperties { get; set; }
    }
}