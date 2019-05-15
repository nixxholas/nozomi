using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Logging;

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

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                    && RequestType >= 0);
        }
    }
}