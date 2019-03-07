using System.Collections.Generic;
using Nozomi.Analysis.Base.Domain.Responses.Hub.Asset;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedRequestEvent
    {
        ICollection<AssetResponse> GetAll(long index = 0, bool active = true);
    }
}