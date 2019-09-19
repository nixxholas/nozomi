// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Services.Address;
using Nozomi.Infra.Blockchain.Auth.Events;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);
                
                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + Environment.MachineName);
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseNpgsql(str));
                var coreStr = Configuration.GetConnectionString("LocalCore:" + Environment.MachineName);
                services.AddDbContext<NozomiDbContext>(options =>
                    options.UseNpgsql(coreStr));
            }
            else
            {
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
                services.AddDbContext<NozomiDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultCoreConnection")));
            }

            services
                .AddEntityFrameworkNpgsql()
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<User>();

            if (HostingEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                //builder.AddSigningCredential();
                throw new Exception("need to configure key material");
            }

            services.AddAuthorization();

            if (HostingEnvironment.IsDevelopment())
            {
                services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication("Bearer", opt =>
                    {
                        opt.Authority = "https://localhost:5001";
                        opt.ApiName = "nozomiapi";
                    });
                    // .AddGoogle(options =>
                    // {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    // options.ClientId = "copy client ID from Google here";
                    // options.ClientSecret = "copy client secret from Google here";
                    // })
                    ;
            }
            else
            {
                services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication("Bearer", opt =>
                    {
                        opt.Authority = "https://auth.nozomi.one";
                        opt.ApiName = "nozomiapi";
                    });
            }

            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();
            services.AddTransient<IDbContext, AuthDbContext>();
            services.AddTransient<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddTransient<IDbContext, NozomiDbContext>();

            services.AddScoped<IValidatingEvent, ValidatingEvent>();
            
            services.AddTransient<IAddressService, AddressService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseAutoDbMigration(HostingEnvironment);
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();
            
            // "default", "{controller=Home}/{action=Index}/{id?}"
            app.UseMvcWithDefaultRoute();
        }
    }
}