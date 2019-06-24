using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Admin.Domain.AreaModels.Exchange;
using Nozomi.Base.Core.Helpers.Enumerator;
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
        /// </summary>
        /// <param name="createExchange"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Initialise(CreateExchange createExchange, long userId = 0)
        {
            if (createExchange != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(createExchange.Endpoint);

                    switch (createExchange.RequestType)
                    {
                        case RequestType.HttpGet:
                            var getResult = await httpClient.GetAsync("");

                            if (getResult.IsSuccessStatusCode)
                            {
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

                                var getToken = JToken.Parse(content);
                                
                                // Interact with the payload
                            }

                            // Failure
                            _logger.LogWarning($"[{_serviceName}] Initialise: Get request failed => " +
                                               $"{getResult.ReasonPhrase}");
                            return false;
                    }
                }
            }
            
            throw new System.NotImplementedException();
        }
    }
}