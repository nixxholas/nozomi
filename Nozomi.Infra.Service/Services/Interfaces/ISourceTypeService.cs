using Nozomi.Data.ResponseModels.SourceType;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ISourceTypeService
    {
        void Create(CreateSourceTypeViewModel vm, string userId);
    }
}