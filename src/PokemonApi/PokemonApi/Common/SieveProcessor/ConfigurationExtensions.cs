using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;

namespace PokemonApi.Common.SieveProcessor
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddSieveProcessor(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SieveOptions>(o => configuration.GetSection("Sieve").Bind(o));

            services.AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            return services;
        }
    }
}
