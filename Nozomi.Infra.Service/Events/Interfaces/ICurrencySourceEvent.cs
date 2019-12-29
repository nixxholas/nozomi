namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencySourceEvent
    {
        bool Exists(long sourceId, long currencyId);
    }
}