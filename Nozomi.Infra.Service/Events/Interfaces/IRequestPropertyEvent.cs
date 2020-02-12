using System.Collections.Generic;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestPropertyEvent
    {
        IEnumerable<RequestPropertyViewModel> GetByRequest(string requestGuid, string validatingUserId);
    }
}