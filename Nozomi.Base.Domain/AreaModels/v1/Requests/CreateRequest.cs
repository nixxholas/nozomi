using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Nozomi.Base.Core;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.AreaModels.v1.Requests
{
    public class CreateRequest
    {
        [Required]
        public RequestType RequestType { get; set; }
        
        [Required]
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        [Required]
        public string DataPath { get; set; }
        
        /// <summary>
        /// Defines the delay of repeating in milliseconds.
        /// </summary>
        [Required]
        public int Delay { get; set; }
        
        public long FailureDelay { get; set; }
        
        public ICollection<CreateCurrencyPairComponent> RequestComponents { get; set; }
        
        public ICollection<CreateRequestProperty> RequestProperties { get; set; }
        
        public long? CurrencyId { get; set; }
        
        public long? CurrencyPairId { get; set; }
        
        public long? CurrencyTypeId { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                    && RequestType >= 0);
        }
    }
}