using System;

namespace Nozomi.Infra.Api.Limiter.Services.Interfaces
{
    public interface INozomiRedisService
    {
        void Add(NozomiRedisDatabase databaseEnum, string key, string value);

        void Remove(NozomiRedisDatabase databaseEnum, string key);
    }
}