using System;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.ComponentHistoricItem;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IComponentHistoricItemService
    {
        bool Push(Component rc);

        void Remove(string guid, string userId = null, bool hardDelete = false);

        void Remove(Guid guid, string userId = null, bool hardDelete = false);

        void Remove(ComponentHistoricItem componentHistoricItem, string userId = null, bool hardDelete = false);

        void Update(UpdateComponentHistoricItemInputModel vm, string userId = null);
    }
}