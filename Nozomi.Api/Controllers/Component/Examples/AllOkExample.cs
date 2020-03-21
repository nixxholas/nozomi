using System;
using System.Collections.Generic;
using System.Globalization;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Component.Examples
{
    public class AllOkExample : IExamplesProvider<ICollection<ComponentViewModel>>
    {
        public ICollection<ComponentViewModel> GetExamples()
        {
            return new List<ComponentViewModel>
            {
                new ComponentViewModel
                {
                    Guid = Guid.NewGuid(),
                    Type = new Random().Next(int.MaxValue),
                    IsDenominated = true,
                    History = new List<ComponentHistoricItemViewModel>
                    {
                        new ComponentHistoricItemViewModel
                        {
                            Timestamp = DateTime.UtcNow,
                            Value = new Random().GenerateDecimal().ToString(CultureInfo.InvariantCulture)
                        },
                        new ComponentHistoricItemViewModel
                        {
                            Timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)),
                            Value = new Random().GenerateDecimal().ToString(CultureInfo.InvariantCulture)
                        }
                    }
                },
                new ComponentViewModel
                {
                    Guid = Guid.NewGuid(),
                    Type = new Random().Next(int.MaxValue),
                    IsDenominated = false,
                    History = new List<ComponentHistoricItemViewModel>
                    {
                        new ComponentHistoricItemViewModel
                        {
                            Timestamp = DateTime.UtcNow,
                            Value = "You can poll strings too"
                        },
                        new ComponentHistoricItemViewModel
                        {
                            Timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)),
                            Value = "Just like how this works"
                        }
                    }
                }
            };
        }
    }
}