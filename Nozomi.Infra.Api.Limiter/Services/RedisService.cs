using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class RedisService : BaseService<RedisService, AuthDbContext>, IRedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public RedisService(ILogger<RedisService> logger, AuthDbContext context, 
            IConnectionMultiplexer connectionMultiplexer) : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public RedisService(IHttpContextAccessor contextAccessor, ILogger<RedisService> logger, AuthDbContext context, 
            IConnectionMultiplexer connectionMultiplexer) 
            : base(contextAccessor, logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
    }
}