using System;
using System.Collections.Generic;
using System.Globalization;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.AnalysedHistoricItem.Examples
{
    public class AllOkExample : IExamplesProvider<ICollection<AnalysedHistoricItemViewModel>>
    {
        public ICollection<AnalysedHistoricItemViewModel> GetExamples()
        {
            return new List<AnalysedHistoricItemViewModel>
            {
                new AnalysedHistoricItemViewModel
                {
                    Timestamp = DateTime.UtcNow,
                    Value = new Random().GenerateDecimal().ToString(CultureInfo.InvariantCulture)
                },
                new AnalysedHistoricItemViewModel
                {
                    Timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(5)),
                    Value = new Random().GenerateDecimal().ToString(CultureInfo.InvariantCulture)
                }
            };
        }
    }
}