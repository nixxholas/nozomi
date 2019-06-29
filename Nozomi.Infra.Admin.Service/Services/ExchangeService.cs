using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Admin.Domain.AreaModels.Exchange;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Services
{
    public class ExchangeService : BaseService<ExchangeService, NozomiDbContext>, IExchangeService
    {
        public ExchangeService(ILogger<ExchangeService> logger, IUnitOfWork<NozomiDbContext> unitOfWork)
            : base(logger, unitOfWork)
        {
        }

        /// <summary>
        /// Exchange initialisation API.
        ///
        /// This API assumes polling an API that returns a collection of the exchange's supporting ticker pairs
        /// and proceeds to process it according to the data given.
        ///
        /// TODO: Support for POST and Websocket requests.
        /// </summary>
        /// <param name="createExchange"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Initialise(CreateExchange createExchange, long userId = 0)
        {
            if (createExchange != null)
            {
                // Setup the source first
                var source = _unitOfWork.GetRepository<Source>()
                    .GetQueryable()
                    .SingleOrDefault(s => s.Abbreviation.Equals(createExchange.SourceAbbreviation));

                // If the source doesn't exist
                if (source == null)
                {
                    // Create it
                    _unitOfWork.GetRepository<Source>().Add(new Source
                    {
                        Abbreviation = createExchange.SourceAbbreviation,
                        Name = createExchange.SourceName,
                        IsEnabled = true
                    });
                    _unitOfWork.Commit(userId);

                    // Set it
                    source = _unitOfWork.GetRepository<Source>()
                        .GetQueryable()
                        .SingleOrDefault(s => s.Abbreviation.Equals(createExchange.SourceAbbreviation));
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(createExchange.Endpoint);
                    var payloadToken = JToken.Parse("");

                    switch (createExchange.RequestType)
                    {
                        case RequestType.HttpGet:
                            var getResult = await httpClient.GetAsync("");

                            if (!getResult.IsSuccessStatusCode)
                                // Failure
                                _logger.LogWarning($"[{_serviceName}] Initialise: Get request failed => " +
                                                   $"{getResult.ReasonPhrase}");

                            var content = await getResult.Content.ReadAsStringAsync();

                            // Parse the content
                            if (getResult.Content.Headers.ContentType.MediaType.Equals(ResponseType.Json
                                .GetDescription()))
                            {
                                // No action needed
                            }
                            else if (getResult.Content.Headers.ContentType.MediaType.Equals(
                                ResponseType.XML.GetDescription()))
                            {
                                // Load the XML
                                createExchange.ResponseType = ResponseType.XML;
                                var xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(content);
                                content = JsonConvert.SerializeObject(xmlDoc);
                            }

                            payloadToken = JToken.Parse(content);

                            break;
                        case RequestType.HttpPost:
                            return false;
                        case RequestType.WebSocket:
                            return false;
                    }

                    // Interact with the payload
                    if (payloadToken.HasValues)
                    {
                        // If its an array,
                        if (payloadToken is JArray)
                        {
                            // Begin iterating
                            foreach (var itemToken in payloadToken)
                            {
                                // 1. Identify the ticker pair
                                
                                // What if the ticker pair has multiple properties for it to be successfully identified?
                                var tickerPairIdentifiers = createExchange.CurrencyPairIdentifier
                                    .Split(" && "); // Split via " && ".
                                string tickerPairStr;
                                 
                                if (tickerPairIdentifiers.Length == 1)
                                    tickerPairStr = itemToken.SelectToken(createExchange.CurrencyPairIdentifier).ToString();
                                else if (tickerPairIdentifiers.Length > 1)
                                {
                                    // Let's loop through the collection and work out the data.
                                    tickerPairStr = "";
                                    foreach (var identifer in tickerPairIdentifiers)
                                    {
                                        tickerPairStr += itemToken.SelectToken(identifer).ToString();
                                    }
                                }

                                // 2. Ensure the currencies exist for this source
                                

                                // 3. Create the ticker pair

                                // 4. Create the components
                            }
                        }
                        else
                        {
                        }
                    }
                }
            }

            // Failure
            _logger.LogWarning($"[{_serviceName}] Initialise: empty payload or request.");
            return false;
        }
    }
}