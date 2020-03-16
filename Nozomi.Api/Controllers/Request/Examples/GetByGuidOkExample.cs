using System;
using System.Collections.Generic;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Data.ViewModels.RequestProperty;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Request.Examples
{
    public class GetByGuidOkExample : IExamplesProvider<RequestViewModel>
    {
        public RequestViewModel GetExamples()
        {
            return new RequestViewModel
            {
                Guid = Guid.NewGuid(),
                IsEnabled = true,
                RequestType = RequestType.HttpGet,
                ResponseType = ResponseType.Json,
                DataPath = "https://api.nozomi.one/api/connect/validate",
                Delay = 10000,
                FailureDelay = 60000,
                ParentType = CreateRequestViewModel.RequestParentType.None,
                Components = new List<ComponentViewModel>
                {
                    new ComponentViewModel
                    {
                        Guid = Guid.NewGuid(),
                        Type = 1,
                        IsDenominated = true,
                        History = new List<ComponentHistoricItemViewModel>
                        {
                            new ComponentHistoricItemViewModel
                            {
                                Timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                                Value = "39482"
                            }
                        }
                    }
                },
                Properties = new List<RequestPropertyViewModel>
                {
                    new RequestPropertyViewModel(Guid.NewGuid(), RequestPropertyType.HttpHeader_Authorization,
                        string.Empty, $"Bearer {Randomizer.GenerateRandomCryptographicKey(32)}")
                }
            };
        }
    }
}