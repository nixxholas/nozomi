using System;
using Nozomi.Preprocessing;

namespace Nozomi.Infra.Api.Limiter.Services.Interfaces
{
    public interface INozomiRedisService
    {
        void Add(RedisDatabases databasesEnum, string key, string value);

        void Remove(RedisDatabases databasesEnum, string key);
    }
}