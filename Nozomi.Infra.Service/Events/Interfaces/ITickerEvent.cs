using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ITickerEvent
    {
        DataTableResult<UniqueTickerResponse> GetAllForDatatable(int index = 0);
    }
}