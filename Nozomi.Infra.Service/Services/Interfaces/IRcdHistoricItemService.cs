using Nozomi.Data.WebModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IRcdHistoricItemService
    {
        bool Push(RequestComponentDatum rcd);
    }
}