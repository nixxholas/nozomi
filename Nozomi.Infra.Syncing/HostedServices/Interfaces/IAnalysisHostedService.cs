namespace Nozomi.Infra.Syncing.HostedServices.Interfaces
{
    public interface IAnalysisHostedService<T>
    {
        bool Analyse(T entity);
    }
}