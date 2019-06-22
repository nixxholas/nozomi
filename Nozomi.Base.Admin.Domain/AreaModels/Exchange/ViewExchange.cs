using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Base.Admin.Domain.AreaModels.Exchange
{
    public class ViewExchange
    {
        [DisplayName("Defines the exchange's current request protocol.")]
        public RequestType RequestType { get; set; }
        
        [DisplayName("The URL of the endpoint that stores the entire exchange's ticker pairs.")]
        public string Endpoint { get; set; }
        
        [DisplayName("This stashes the raw data coming from the request.")]
        public string Payload { get; set; }
    }
}