using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class RedisTokenUserService : BaseService<RedisTokenUserService, AuthDbContext>, IRedisTokenUserService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public RedisTokenUserService(ILogger<RedisTokenUserService> logger, AuthDbContext context, 
            IConnectionMultiplexer connectionMultiplexer) : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public RedisTokenUserService(IHttpContextAccessor contextAccessor, ILogger<RedisTokenUserService> logger, 
            AuthDbContext context, IConnectionMultiplexer connectionMultiplexer) 
            : base(contextAccessor, logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
    }
}