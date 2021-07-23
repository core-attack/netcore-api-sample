using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MySql.EntityFrameworkCore.Extensions;
using PokemonApi.Common.DbContext;
using PokemonApi.Common.Providers;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace PokemonApi.Common.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddSingleton<ISettingsProvider, SettingsProvider>()
                .AddTransient<CsvReaderProvider>();

            return services;
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(
                    options =>
                    {
                        options.DescribeAllParametersInCamelCase();
                        options.EnableAnnotations();
                        //options.OperationFilter<ServerErrorOperationFilter>();
                        options.CustomSchemaIds(c => c.FullName);
                        options.IgnoreObsoleteActions();
                        options.IgnoreObsoleteProperties();

                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = "Pokemon API",
                            Version = "v1"
                        });

                        // Configure Swagger to use the xml documentation file
                        var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                        options.IncludeXmlComments(xmlFile);
                    })
                .AddSwaggerGenNewtonsoftSupport();

        public static IServiceCollection AddDbContext(this IServiceCollection services) =>
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

        public static IServiceCollection AddCustomMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(Startup).Assembly);
        }

    }

}
