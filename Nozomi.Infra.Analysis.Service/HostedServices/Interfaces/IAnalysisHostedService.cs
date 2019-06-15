namespace Nozomi.Infra.Analysis.Service.HostedServices.Interfaces
{
    public interface IAnalysisHostedService<T>
    {
        bool Analyse(T entity);
    }
}