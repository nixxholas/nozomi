using System;
namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairComponentService
    {
        bool UpdatePairValue(long id, decimal val);
    }
}
