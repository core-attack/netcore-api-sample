using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace PokemonApi.Common.SieveProcessor
{
    public class ApplicationSieveProcessor : Sieve.Services.SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options, ISieveCustomSortMethods sorts, ISieveCustomFilterMethods filters)
            : base(options, sorts, filters)
        {
        }
    }
}
