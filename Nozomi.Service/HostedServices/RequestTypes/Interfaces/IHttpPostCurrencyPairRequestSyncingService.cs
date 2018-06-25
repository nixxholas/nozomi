using Nozomi.Data.WebModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpPostCurrencyPairRequestSyncingService
    {
        Task<bool> Process(CurrencyPairRequest req);
    }
}
