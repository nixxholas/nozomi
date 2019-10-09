using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Requests;

namespace Nozomi.Ticker.Areas.Admin.Controllers.Request
{
    public class RequestViewModel
    {
        public RequestDTO Request { get; set; }
        public List<KeyValuePair<string, int>> RequestTypes { get; set; }
        
        public List<KeyValuePair<string, int>> ResponseTypes { get; set; }
        
        public List<KeyValuePair<string, int>> RequestComponentTypes { get; set; }
    }
}