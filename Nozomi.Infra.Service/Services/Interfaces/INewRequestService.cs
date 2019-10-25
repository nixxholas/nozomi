using Nozomi.Service.ViewModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface INewRequestService
    {
        void Create(CreateRequestViewModel vm);
    }
}