﻿using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Squeaker.Application;

namespace Squeaker.AcceptanceTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's SqueakerContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<SqueakerContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<SqueakerContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (SqueakerContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SqueakerContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    SeedData(db);
                }
            });
        }

        private void SeedData(SqueakerContext dbContext)
        {
            dbContext.Squeakes.RemoveRange(dbContext.Squeakes);
            dbContext.SaveChanges();
            dbContext.Squeakes.AddRange(new[]
            {
                new Squeake
                {
                    Id = "39700594",
                    Text = "Cum sociis natoque penatibus et"
                },
                new Squeake
                {
                    Id = "ea90b094",
                    Text = "Nam a sapien"
                },
                new Squeake
                {
                    Id = "1028bde8",
                    Text = "Praesent augue Sed bibendum."
                }
            });
            dbContext.SaveChanges();
        }
    }
}