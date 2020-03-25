using System;
using System.Collections.Generic;
using System.Globalization;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Nozomi.Data.ViewModels.ComponentType;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.ComponentType.Examples
{
    public class AllOkExample : IExamplesProvider<IEnumerable<ComponentTypeViewModel>>
    {
        public IEnumerable<ComponentTypeViewModel> GetExamples()
        {
            return new List<ComponentTypeViewModel>
            {
                new ComponentTypeViewModel
                {
                    Id = new Random().GenerateInt32(),
                    Slug = "TEST",
                    Name = "Test Type",
                    IsEnabled = true,
                    Description = "Just a cute sample for u",
                    Components = new List<ComponentViewModel>
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
                        }
                    }
                }
            };
        }
    }
}