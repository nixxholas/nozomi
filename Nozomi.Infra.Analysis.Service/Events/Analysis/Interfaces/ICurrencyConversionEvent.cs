using System.Collections.Generic;
using Nozomi.Data;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface ICurrencyConversionEvent
    {
        NozomiResult<IDictionary<string, decimal>> ObtainConversionRates(string abbrv);
    }
}