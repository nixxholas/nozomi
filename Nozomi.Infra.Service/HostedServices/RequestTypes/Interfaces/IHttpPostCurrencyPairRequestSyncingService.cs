using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpPostCurrencyPairRequestSyncingService
    {
        Task<bool> Process(CurrencyPairRequest req);
    }
}
