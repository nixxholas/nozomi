using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Base.Admin.Domain.AreaModels.Exchange
{
    /// <summary>
    /// CreateExchange Struct.
    ///
    /// TODO: This will not work if RequestProperties have to be dynamic. Let's figure something about that.
    /// </summary>
    public class CreateExchange : ViewExchange
    {
        public IEnumerable<RequestType> RequestTypes { get; }
            = Enum.GetValues(typeof(RequestType)).Cast<RequestType>();

        public IEnumerable<ResponseType> ResponseTypes { get; }
            = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>();
        
        public string SourceName { get; set; }
        
        [Required]
        public string SourceAbbreviation { get; set; }
        
        /// <summary>
        /// Ensure that the user provides an API that exposes the entire exchange's set of ticker pairs.
        /// </summary>
        [Required]
        public string SingularityApiUrl { get; set; }
        
        /// <summary>
        /// This behaves like a header, allowing us to identify the ticker pair dynamically, without the
        /// need of defining every single ticker pair to be seeded manually.
        /// </summary>
        [Required]
        public string CurrencyPairIdentifier { get; set; }
        
        /// <summary>
        /// Defines the Query components that will be created per-ticker pair.
        ///
        /// Every line defines a new component, let's see how we can design the structure properly,
        /// 1. Obtain the ticker pair from the payload
        /// 
        /// </summary>
        [Required]
        public string QueryComponents { get; set; }
        
        public string RequestProperties { get; set; }
    }
}