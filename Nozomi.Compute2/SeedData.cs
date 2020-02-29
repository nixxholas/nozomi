// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Data.Models.Web;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Compute2
{
    public static class SeedData
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetService<NozomiComputeDbContext>())
                {
                    if (env.IsDevelopment())
                    {
                        dbContext.Database.EnsureDeleted();
                        
                        dbContext.Database.Migrate();
                        
                        if (!dbContext.Computes.Any())
                        {
                            var simpleMultiplicationCompute = new Compute()
                            {
                                Formula = "4 * 3",
                                Delay = 60000
                            };

                            dbContext.Computes.Add(simpleMultiplicationCompute);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}