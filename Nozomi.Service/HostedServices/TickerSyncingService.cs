using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Counter.SDK.Utils;
using Counter.SDK.Utils.Numerics;
using Microsoft.Extensions.Hosting;
using Nozomi.Data.CurrencyModels;
using Nozomi.Service.HostedServices.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.HostedServices
{
    public class TickerSyncingService : BaseHostedService<TickerSyncingService>
        , ITickerSyncingService, IHostedService, IDisposable
    {
        private ICurrencyPairService _cpService;
        private ICurrencyPairComponentService _cpcService;

        public TickerSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _cpService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairService>();
            _cpcService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairComponentService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TickerSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("TickerSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("TickerSyncingService is doing background work.");

                // Retrieve all the active currency pairs.
                var cPairs = _cpService.GetAllActive();

                // Poll data.
                Dictionary<long, decimal> result = await PollDataFromExternalsAsync(cPairs);

                // Process and update it
                foreach (var kvPair in result)
                {
                    _cpcService.UpdatePairValue(kvPair.Key, kvPair.Value);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _logger.LogWarning("TickerSyncingService background task is stopping.");
        }

        public async Task<Dictionary<long, decimal>> PollDataFromExternalsAsync(IEnumerable<CurrencyPair> cPairs)
        {
            // Instantiate a dictionary
            Dictionary<long, decimal> dataSet = new Dictionary<long, decimal>();
            Dictionary<string, string> apiPayload = new Dictionary<string, string>();

            var apiUrls = _cpService.GetAllCurrencyPairUrls();

            // Query all the APIs first
            foreach (var apiUrl in apiUrls)
            {
                if (!apiPayload.ContainsKey(apiUrl))
                {
                    // Setup the HTTP Client
                    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
                    // 
                    // In this case, we're assuming that CounterCore is always connected
                    // to the internet. Take note of this 'possible' issue
                    // https://stackoverflow.com/questions/28537916/check-internet-connection-available-or-not-in-c-sharp
                    using (var client = new HttpClient())
                    {
                        // GET it
                        var httpRes = await client.GetAsync(apiUrl);

                        if (httpRes.IsSuccessStatusCode)
                        {
                            var httpContent = await httpRes.Content.ReadAsStringAsync();

                            apiPayload.Add(apiUrl, httpContent);
                        }
                    }
                }
            }

            // Foreach currency pair in the dataset
            foreach (var pair in cPairs)
            {
                if (!string.IsNullOrEmpty(apiPayload[pair.APIUrl]))
                {
                    var httpContent = apiPayload[pair.APIUrl];

                    var jsonToken = JToken.Parse(httpContent);

                    // Is it an array or an object?
                    // https://stackoverflow.com/questions/20620381/determine-if-json-results-is-object-or-array
                    if (jsonToken is JArray)
                    {
                        // Pump in the array
                        List<string> dataList = jsonToken.ToObject<List<string>>();

                        // If the db really hodls a number,
                        foreach (var component in pair.CurrencyPairComponents)
                        {
                            if (component.QueryComponent != null &&
                                int.TryParse(component.QueryComponent, out int index))
                            {
                                // let's work it out
                                if (index >= 0 && index < dataList.Count)
                                {
                                    // Number checks
                                    // Make sure the datalist element we're targetting contains a proper value.
                                    if (decimal.TryParse(dataList[index], out decimal val))
                                    {
                                        // Add the value in
                                        dataSet.Add(component.Id, val);
                                    }
                                }
                            }
                        }
                    }
                    else if (jsonToken is JObject)
                    {
                        // Pump in the object
                        JObject obj = jsonToken.ToObject<JObject>();

                        foreach (var component in pair.CurrencyPairComponents)
                        {
                            if (component.QueryComponent != null)
                            {
                                var rawData = (string) obj.SelectToken(component.QueryComponent);

                                if (rawData != null)
                                {
                                    // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
                                    var style = NumberStyles.Any;
                                    if (ExponentHelper.IsExponentialFormat(rawData))
                                    {
                                        style = NumberStyles.Float;
                                    }

                                    // If it is an exponent
                                    if (decimal.TryParse(rawData, style, CultureInfo.InvariantCulture,
                                        out decimal val))
                                    {
                                        if (val > 0)
                                        {
                                            dataSet.Add(pair.Id, val);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Goodbye
            return dataSet;
        }
    }
}