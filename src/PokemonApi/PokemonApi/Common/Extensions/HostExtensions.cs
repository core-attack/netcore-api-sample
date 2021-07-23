using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PokemonApi.Common.DbContext;
using PokemonApi.Common.Providers;
using Polly;

public static class HostExtensions
{
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(new[]
                    {
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromSeconds(15),
                    });

                retry.Execute(() =>
                {
                    context.Database.Migrate();

                    seeder(context, services);
                });

                logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
                throw;
            }
        }

        return host;
    }

    public static async Task<IHost> SeedData(this IHost host,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (host == null)
        {
            throw new ArgumentNullException(nameof(host));
        }

        using var scope = host.Services.CreateScope();

        var reader = scope.ServiceProvider.GetService<CsvReaderProvider>();

        await reader.Read(cancellationToken);

        return host;
    }

    public static void SeedData(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        using var scope = serviceProvider.CreateScope();

        var reader = scope.ServiceProvider.GetService<CsvReaderProvider>();

        Task.WhenAll(reader.Read(CancellationToken.None));
    }
}