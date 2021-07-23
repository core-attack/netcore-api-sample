using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using PokemonApi.Common.Exceptions;

namespace PokemonApi.Common.Providers
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IConfiguration configuration;
        private const int DefaultBatchSize = 10;

        public SettingsProvider(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string ConnectionString()
        {
            var connectionString = configuration["ConnectionString"]?.Trim();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new BusinessException("ConnectionString is not defined", HttpStatusCode.UnprocessableEntity, ErrorCodes.ConnectionStringIsNotDefined);
            }

            return connectionString;
        }

        public string DefaultCsvFilePath()
        {
            var result = configuration["DefaultCsvFilePath"]?.Trim();

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new BusinessException("DefaultCsvFilePath is not defined", HttpStatusCode.UnprocessableEntity, ErrorCodes.DefaultCsvFilePathIsNotDefined);
            }

            return result;
        }

        public int BatchSize()
        {
            var result = configuration["BatchSize"]?.Trim();
            var size = DefaultBatchSize;
            
            int.TryParse(result, out size);

            return size;
        }
    }
}
