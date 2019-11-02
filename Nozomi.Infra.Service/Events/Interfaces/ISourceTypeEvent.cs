using System.Collections.Generic;
using Nozomi.Data.ResponseModels.SourceType;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceTypeEvent
    {
        IEnumerable<SourceTypeViewModel> GetAll(bool track = false);
    }
}