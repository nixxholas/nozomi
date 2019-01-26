﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpGetCurrencyPairRequestSyncingService
    {
        Task<bool> ProcessByDataPath(ICollection<CurrencyPairRequest> requests);

        bool Update(JToken currToken, ResponseType resType, IEnumerable<RequestComponent> requestComponents);
    }
}