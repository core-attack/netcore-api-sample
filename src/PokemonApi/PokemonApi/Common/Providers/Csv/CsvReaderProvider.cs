using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using CsvHelper;
using PokemonApi.Common.DbContext;
using PokemonApi.Common.Exceptions;
using PokemonApi.Common.Extensions;
using PokemonApi.Common.Providers.Csv;
using PokemonApi.Models;

namespace PokemonApi.Common.Providers
{
    public class CsvReaderProvider
    {
        private readonly IMapper mapper;
        private readonly ISettingsProvider settings;
        private readonly PokemonDbContext dbContext;

        public CsvReaderProvider(
            IMapper mapper, 
            ISettingsProvider settings, 
            PokemonDbContext dbContext)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Read(CancellationToken cancellationToken)
        {
            var path = settings.DefaultCsvFilePath();

            if (!File.Exists(path))
            {
                throw new BusinessException("Default csv file is not exist", HttpStatusCode.UnprocessableEntity, ErrorCodes.DefaultCsvFilePathIsNotExist);
            }

            var count = dbContext.Set<Pokemon>().Count();

            if (count == 0)
            {
                foreach (IEnumerable<PokemonModel> batch in ReadBatches(path, settings.BatchSize()))
                {
                    await FillArchive(batch, cancellationToken);
                    await ApplyModifications(batch, cancellationToken);
                }
            }
        }

        private async Task FillArchive(IEnumerable<PokemonModel> records, CancellationToken cancellationToken)
        {
            var mapped = records.Select(x => mapper.Map<PokemonModel, PokemonArchive>(x));

            dbContext.AddRange(mapped);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ApplyModifications(IEnumerable<PokemonModel> records, CancellationToken cancellationToken)
        {
            //Exclude Legendary Pokémon
            records = records.Where(x => !x.Legendary);

            Func<PokemonModel, string, bool> withType = (x, type) =>
                x.Type1.ToUpperInvariant().Contains(type) || x.Type2.ToUpperInvariant().Contains(type);

            //Exclude Pokémon of Type: Ghost
            var excludedType = "Ghost".ToUpperInvariant();
            records = records.Where(x => !withType(x, excludedType));

            //For Pokémon of Type: Steel, double their HP
            var steelType = "Steel".ToUpperInvariant();
            var steelPokemons = records.Where(x => withType(x, steelType));
            steelPokemons.ForAll(x => x.Hp = x.Hp * 2);

            //For Pokémon of Type: Fire, lower their Attack by 10 %
            var fireType = "Fire".ToUpperInvariant();
            var firePokemons = records.Where(x => withType(x, fireType));
            firePokemons.ForAll(x => x.Attack = x.Attack * 0.9);

            //For Pokémon of Type: Bug & Flying, increase their Attack Speed by 10 %
            var bugType = "Bug".ToUpperInvariant();
            var flyingType = "Flying".ToUpperInvariant();
            var bugAndFlyingPokemons = records.Where(x => withType(x, bugType) && withType(x, flyingType));
            bugAndFlyingPokemons.ForAll(x => x.AttackSpeed = x.AttackSpeed * 1.1);

            //For Pokémon that start with the letter **G * *, add + 5 Defense for every letter in their name(excluding * *G * *)
            var sub = "G";
            var gPokemons = records.Where(x => x.Name.ToUpperInvariant().StartsWith(sub));
            gPokemons.ForAll(x => x.Defense += 5 * (x.Name.Length - x.Name.CountOf(sub)));//assume that we exclude all "G" letters

            var mapped = records.Select(x => mapper.Map<PokemonModel, Pokemon>(x));

            //I assume that we don't need not unique identifiers in DB, so the id column (#) will be replaced with auto-incremented data.

            dbContext.AddRange(mapped);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private IEnumerable<IEnumerable<PokemonModel>> ReadBatches(string path, int batchSize)
        {
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<PokemonModelMap>();

            var batchItems = new List<PokemonModel>();
            var ok = csv.Read();
            csv.ReadHeader();

            while (ok)
            {
                batchItems.Clear();

                for (int i = 0; i < batchSize; i++)
                {
                    ok = csv.Read();
                    if (!ok)
                    {
                        break;
                    }

                    batchItems.Add(csv.GetRecord<PokemonModel>());
                }

                yield return batchItems;
            }
        }
    }
}
