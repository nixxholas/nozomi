﻿using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.HubModels.Interfaces
{
    public interface ITickerHubClient
    {
        Task<ChannelReader<NozomiResult<CurrencyPair>>> SubscribeToAll();
        void BroadcastData(JObject data);
    }
}