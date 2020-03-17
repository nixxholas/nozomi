using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.AnalysedComponent.Examples
{
    public class GetOkExample : IExamplesProvider<AnalysedComponentViewModel>
    {
        public AnalysedComponentViewModel GetExamples()
        {
            return new AnalysedComponentViewModel
            {
                Guid = Guid.NewGuid(),
                Type = AnalysedComponentType.BorrowRate,
                CurrencyPairGuid = Guid.NewGuid().ToString(),
                CurrencySlug = "usd",
                CurrencyTypeShortForm = "fiat",
                Delay = 5000,
                IsDenominated = true,
                IsEnabled = true,
                StoreHistoricals = true,
                History = new List<AnalysedHistoricItemViewModel>
                {
                    new AnalysedHistoricItemViewModel
                    {
                        Timestamp = DateTime.UtcNow,
                        Value = "1.02"
                    },
                    new AnalysedHistoricItemViewModel
                    {
                        Timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                        Value = "1.025"
                    }
                },
                UiFormatting = "0[.]00000"
            };
        }
    }
}