using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IRequestComponentService
    {
        NozomiResult<string> Create(CreateRequestComponent createRequestComponent, long userId = 0);
        
        NozomiResult<string> UpdatePairValue(long id, decimal val);
        NozomiResult<string> UpdatePairValue(long id, string val);

        NozomiResult<string> Update(UpdateRequestComponent updateRequestComponent, long userId = 0);

        NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false);
    }
}
