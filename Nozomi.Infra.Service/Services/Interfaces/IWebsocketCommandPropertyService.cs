using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IWebsocketCommandPropertyService
    {
        void Create(CreateWebsocketCommandPropertyInputModel vm, string userId);

        void Update(UpdateWebsocketCommandPropertyInputModel vm, string userId);

        void Delete(string propertyGuid, string userId, bool hardDelete = true);
        
        void Delete(long propertyId, string userId, bool hardDelete = true);
    }
}