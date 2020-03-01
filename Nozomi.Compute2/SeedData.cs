// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
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
                                Delay = 60000,
                                CreatedAt = DateTime.UtcNow,
                                ModifiedAt = DateTime.UtcNow
                            };

                            dbContext.Computes.Add(simpleMultiplicationCompute);
                            dbContext.SaveChanges();
                            
                            var basicMultiplicationCompute = new Compute
                            {
                                Formula = "[x] * [y]",
                                Delay = 5000,
                                CreatedAt = DateTime.UtcNow,
                                ModifiedAt = DateTime.UtcNow,    
                                Expressions = new List<ComputeExpression>
                                {
                                    new ComputeExpression
                                    {
                                        Type = ComputeExpressionType.Generic,
                                        Expression = "x",
                                        Value = "5"
                                    },
                                    new ComputeExpression
                                    {
                                        Type = ComputeExpressionType.Generic,
                                        Expression = "y",
                                        Value = "5"
                                    }
                                }
                            };

                            dbContext.Computes.Add(basicMultiplicationCompute);
                            dbContext.SaveChanges();

                            var twoTierMultiplicationCompute = new Compute
                            {
                                Formula = $"[{simpleMultiplicationCompute.Guid}] * 5",
                                Delay = 5000,
                                CreatedAt = DateTime.UtcNow,
                                ModifiedAt = DateTime.UtcNow,
                                ChildComputes = new List<SubCompute>
                                {
                                    new SubCompute
                                    {
                                        ChildComputeGuid = basicMultiplicationCompute.Guid
                                    }
                                }
                            };

                            dbContext.Computes.Add(twoTierMultiplicationCompute);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}