using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.EntityFrameworkCore.Extensions;
using PokemonApi.Common.DbContext;
using PokemonApi.Common.Providers;

namespace Tests.Common
{
    public class PokemonFixture : IDisposable
    {
        public PokemonFixture()
        {
            Factory = new ApiHostFactory();
            Client = Factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("environment", "Development");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PokemonDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services
                        .AddEntityFrameworkMySQL()
                        .AddDbContext<PokemonDbContext>(
                            (sp, o) =>
                            {
                                var settings = sp.GetService<ISettingsProvider>();
                                var hostingEnvironment = sp.GetService<IWebHostEnvironment>();
                                var environmentName = hostingEnvironment?.EnvironmentName ?? string.Empty;

                                o.UseMySQL(
                                        settings.ConnectionString(),
                                        optionsBuilder =>
                                        {
                                            optionsBuilder.MigrationsAssembly(
                                                typeof(PokemonDbContext).GetTypeInfo().Assembly.GetName().Name);
                                        })
                                    .EnableSensitiveDataLogging(string.Equals(Environments.Development, environmentName, StringComparison.Ordinal))
                                    .EnableDetailedErrors();
                                ;
                                o.UseInternalServiceProvider(sp);
                            });

                    ServiceProviderInstance = services.BuildServiceProvider();
                    ServiceProviderInstance.SeedData();
                });
            })
                .CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = false });

            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));
        }

        public ServiceProvider ServiceProviderInstance { get; private set; }

        public ApiHostFactory Factory { get; private set; }

        public HttpClient Client { get; private set; }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
            }

            if (Factory != null)
            {
                Factory.Dispose();
            }
        }
    }
}
