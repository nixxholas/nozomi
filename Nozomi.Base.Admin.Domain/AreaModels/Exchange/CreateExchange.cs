using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Base.Admin.Domain.AreaModels.Exchange
{
    public class CreateExchange : ViewExchange
    {
        public IEnumerable<RequestType> RequestTypes { get; }
            = Enum.GetValues(typeof(RequestType)).Cast<RequestType>();
        
        public string QueryComponents { get; set; }
        
        public string RequestProperties { get; set; }
    }
}