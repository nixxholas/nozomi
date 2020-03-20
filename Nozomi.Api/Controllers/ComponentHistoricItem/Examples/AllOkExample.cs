using System;
using System.Collections.Generic;
using System.Globalization;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.ComponentHistoricItem.Examples
{
    public class AllOkExample : IExamplesProvider<ICollection<ComponentHistoricItemViewModel>>
    {
        public ICollection<ComponentHistoricItemViewModel> GetExamples()
        {
            return new List<ComponentHistoricItemViewModel>
            {
                new ComponentHistoricItemViewModel
                {
                    Timestamp = DateTime.UtcNow,
                    Value = new Random().GenerateDecimal().ToString(CultureInfo.InvariantCulture)
                },
                new ComponentHistoricItemViewModel
                {
                    Timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(5000)),
                    Value = new Random().GenerateDecimal().ToString(CultureInfo.InvariantCulture)
                }
            };
        }
    }
}