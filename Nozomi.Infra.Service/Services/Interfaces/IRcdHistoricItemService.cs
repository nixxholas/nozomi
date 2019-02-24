using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IRcdHistoricItemService
    {
        bool Push(RequestComponentDatum rcd);
    }
}