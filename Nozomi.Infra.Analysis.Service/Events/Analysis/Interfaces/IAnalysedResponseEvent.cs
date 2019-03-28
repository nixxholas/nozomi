using System.Collections.Generic;
using Nozomi.Analysis.Base.Domain.Responses.Hub.Asset;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedResponseEvent
    {
        ICollection<AssetResponse> GetAllAssetResponses(long index = 0);
    }
}