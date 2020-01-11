using Nozomi.Data.ViewModels.SourceType;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ISourceTypeService
    {
        void Create(CreateSourceTypeViewModel vm, string userId);

        bool Update(UpdateSourceTypeViewModel vm, string userId);
    }
}