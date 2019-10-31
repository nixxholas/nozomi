using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentTypeEvent
    {
        ICollection<KeyValuePair<string, int>> GetAllKeyValuePairs();
    }
}