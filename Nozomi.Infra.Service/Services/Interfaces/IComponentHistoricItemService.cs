using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IComponentHistoricItemService
    {
        bool Push(Component rc);

        void Remove(long id, string userId = null, bool hardDelete = false);

        void Remove(ComponentHistoricItem componentHistoricItem, string userId = null, bool hardDelete = false);
    }
}