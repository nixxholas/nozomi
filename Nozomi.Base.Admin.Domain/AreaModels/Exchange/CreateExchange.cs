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
        
        public string SourceName { get; set; }
        
        [Required]
        public string SourceAbbreviation { get; set; }
        
        /// <summary>
        /// Ensure that the user provides an API that exposes the entire exchange's set of ticker pairs.
        /// </summary>
        [Required]
        public string SingularityApiUrl { get; set; }
        
        /// <summary>
        /// Defines the Query components that will be created per-ticker pair.
        /// </summary>
        [Required]
        public string QueryComponents { get; set; }
        
        public string RequestProperties { get; set; }
    }
}