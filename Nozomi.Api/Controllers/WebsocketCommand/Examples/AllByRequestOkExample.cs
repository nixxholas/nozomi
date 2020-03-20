using System;
using System.Collections.Generic;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.WebsocketCommand.Examples
{
    public class AllByRequestOkExample : IExamplesProvider<ICollection<WebsocketCommandViewModel>>
    {
        public ICollection<WebsocketCommandViewModel> GetExamples()
        {
            return new List<WebsocketCommandViewModel>
            {
                new WebsocketCommandViewModel(Guid.NewGuid().ToString(), CommandType.Json, "Command", 5000, 
                    true, new List<WebsocketCommandPropertyViewModel>
                    {
                        new WebsocketCommandPropertyViewModel
                        {
                            Guid = Guid.NewGuid().ToString(), 
                            Type = CommandPropertyType.Default, 
                            Key = "Category",
                            Value = "Ticker", 
                            IsEnabled = true 
                        }
                    }),
                new WebsocketCommandViewModel(Guid.NewGuid().ToString(), CommandType.Json, "Auth", 5000, 
                    true, new List<WebsocketCommandPropertyViewModel>
                    {
                        new WebsocketCommandPropertyViewModel
                        {
                            Guid = Guid.NewGuid().ToString(), 
                            Type = CommandPropertyType.Default, 
                            Key = "api-key",
                            Value = Randomizer.GenerateRandomCryptographicKey(64), 
                            IsEnabled = true 
                        }
                    })
            };
        }
    }
}