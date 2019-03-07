using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Repo.Data;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyRequestEvent
    {
        IDictionary<string, ICollection<CurrencyRequest>> GetAllByRequestTypeUniqueToUrl(
            NozomiDbContext nozomiDbContext, RequestType requestType);
    }
}