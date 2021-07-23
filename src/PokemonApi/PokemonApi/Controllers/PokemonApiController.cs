using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PokemonApi.Common.DbContext;
using PokemonApi.Models;
using Sieve.Models;
using Sieve.Services;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonApiController : ControllerBase
    {
        private readonly ILogger<PokemonApiController> logger;
        private PokemonDbContext dbContext;
        private IMapper mapper;
        private readonly ISieveProcessor sieveProcessor;
        private readonly IOptions<SieveOptions> sieveOptions;

        public PokemonApiController(
            ILogger<PokemonApiController> logger, 
            PokemonDbContext dbContext,
            IMapper mapper,
            ISieveProcessor sieveProcessor,
            IOptions<SieveOptions> sieveOptions)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.sieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
            this.sieveOptions = sieveOptions ?? throw new ArgumentNullException(nameof(sieveOptions));
        }

        /// <summary>
        /// Get all pokemons 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<PokemonModel>), StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<PokemonModel>> Get([FromQuery] SieveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var sieve = BuildSieve(model);

            var q = dbContext.Set<Pokemon>()
                .AsNoTracking();

            var result = sieveProcessor
                .Apply(sieve,q)
                .Select(x => mapper.Map<Pokemon, PokemonModel>(x));

            return await result.ToArrayAsync();
        }

        /// <summary>
        /// Get all pokemons 
        /// </summary>
        /// <returns></returns>
        [HttpGet("archive")]
        [ProducesResponseType(typeof(IReadOnlyCollection<PokemonModel>), StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<PokemonModel>> GetArchive([FromQuery] SieveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var sieve = BuildSieve(model);

            var q = dbContext.Set<PokemonArchive>()
                .AsNoTracking();

            var result = sieveProcessor
                .Apply(sieve, q)
                .Select(x => mapper.Map<PokemonArchive, PokemonModel>(x));

            return await result.ToArrayAsync();
        }

        private SieveModel BuildSieve(SieveModel? sieveModel)
        {
            if (sieveModel == null)
            {
                return new SieveModel();
            }

            var filters = sieveModel
                .GetFiltersParsed()
                ?.Select(f => $"{string.Join('|', f.Names)}{f.Operator}{string.Join('|', f.Values)}");

            return new SieveModel
            {
                Page = sieveModel.Page > 0 ? sieveModel.Page : 1,
                PageSize = sieveModel.PageSize > 0 ? sieveModel.PageSize : sieveOptions.Value.DefaultPageSize,
                Sorts = sieveModel.Sorts,
                Filters = filters is null ? null : string.Join(',', filters)
            };
        }
    }
}
