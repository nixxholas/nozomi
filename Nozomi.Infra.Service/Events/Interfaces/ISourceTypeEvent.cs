using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.SourceType;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceTypeEvent
    {
        SourceType Find(string sourceTypeGuid);
        
        IEnumerable<SourceTypeViewModel> GetAll(bool track = false);
    }
}