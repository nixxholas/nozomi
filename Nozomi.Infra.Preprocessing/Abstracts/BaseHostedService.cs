using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nozomi.Preprocessing.Abstracts
{
    // Copyright (c) .NET Foundation. Licensed under the Apache License, Version 2.0.
    /// <summary>
    /// Base class for implementing a long running <see cref="IHostedService"/>.
    /// </summary>
    public abstract class BaseHostedService<T> : BackgroundService where T : class
    {
        public readonly string _hostedServiceName;
        // Use a service provider factory to access scoped services.
        // https://forums.asp.net/t/2134510.aspx?Inject+dbcontext+in+IHostedService
        protected readonly IServiceScopeFactory _scopeFactory;
        public readonly ILogger<T> _logger;

        protected BaseHostedService(IServiceScopeFactory scopeFactory) {
            _hostedServiceName = typeof(T).Name;
            _scopeFactory = scopeFactory;
            _logger = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILogger<T>>();
        }
    }
}