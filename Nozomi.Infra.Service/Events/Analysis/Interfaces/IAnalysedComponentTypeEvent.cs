using System.Collections.Generic;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentTypeEvent
    {
        ICollection<KeyValuePair<string, int>> GetAllKeyValuePairs();
    }
}