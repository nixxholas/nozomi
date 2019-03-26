using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IPartialCurrencyPairEvent
    {
        ICollection<PartialCurrencyPair> ObtainCounterPCPs(ICollection<PartialCurrencyPair> mainPCPs);
    }
}