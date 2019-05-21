using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;

namespace Nozomi.Base.Identity.ViewModels.Manage.Request
{
    public class RequestViewModel
    {
        public RequestDTO Request { get; set; }
        public List<KeyValuePair<string, int>> RequestTypes { get; set; }
        
        public List<KeyValuePair<string, int>> ResponseTypes { get; set; }
       
    }
}