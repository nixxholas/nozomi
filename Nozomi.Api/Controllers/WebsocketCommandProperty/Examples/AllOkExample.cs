using System;
using System.Collections.Generic;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty.Examples
{
    public class AllOkExample : IExamplesProvider<ICollection<WebsocketCommandPropertyViewModel>>
    {
        public ICollection<WebsocketCommandPropertyViewModel> GetExamples()
        {
            var commandGuid = Guid.NewGuid();
            return new List<WebsocketCommandPropertyViewModel>
            {
                new WebsocketCommandPropertyViewModel
                {
                    Guid = Guid.NewGuid().ToString(), 
                    Type = CommandPropertyType.Default, 
                    Key = "Category",
                    Value = "Ticker", 
                    IsEnabled = true,
                    CommandGuid = commandGuid.ToString()
                },
                new WebsocketCommandPropertyViewModel
                {
                    Guid = Guid.NewGuid().ToString(), 
                    Type = CommandPropertyType.Default, 
                    Key = "api-key",
                    Value = Randomizer.GenerateRandomCryptographicKey(32), 
                    IsEnabled = true,
                    CommandGuid = commandGuid.ToString()
                }
            };
        }
    }
}