namespace Nozomi.Infra.Api.Limiter.Services.Interfaces
{
    public interface IRedisTokenUserService
    {
        void Consume(string userId, long weight = 1);

        void TopUp(string userId, long tokenCount = 1);
    }
}