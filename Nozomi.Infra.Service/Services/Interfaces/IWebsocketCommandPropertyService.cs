using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IWebsocketCommandPropertyService
    {
        void Create(CreateWebsocketCommandPropertyInputModel vm, string userId = null);

        void Update(UpdateWebsocketCommandPropertyInputModel vm, string userId = null);

        void Delete(string propertyGuid, string userId = null, bool hardDelete = true);
        
        void Delete(long propertyId, string userId = null, bool hardDelete = true);
    }
}