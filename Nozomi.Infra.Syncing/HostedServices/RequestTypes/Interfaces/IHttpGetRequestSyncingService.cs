﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Syncing.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpGetRequestSyncingService
    {
        Task<bool> ProcessRequest<T>(IEnumerable<T> requests) where T : Request;

        bool Update(JToken currToken, ResponseType resType, ICollection<Component> requestComponents);
    }
}