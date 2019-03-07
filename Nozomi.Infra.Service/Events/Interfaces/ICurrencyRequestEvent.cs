using System.Collections.Generic;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyRequestEvent
    {
        IDictionary<string, ICollection<CurrencyRequest>> GetAllByRequestTypeUniqueToUrl(RequestType requestType);
    }
}