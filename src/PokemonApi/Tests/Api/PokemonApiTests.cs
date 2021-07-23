using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using PokemonApi.Common.Extensions;
using PokemonApi.Models;
using Tests.Common;
using Xunit;

namespace Tests.Api
{
    [Collection("Pokemon Сollection")]
    public class PokemonApiTests
    {
        private PokemonFixture fixture;
        private PokemonWebClient webClient;

        public PokemonApiTests(PokemonFixture fixture)
        {
            this.fixture = fixture;
            webClient = new PokemonWebClient(fixture.Factory, fixture.Client);
        }

        public static IEnumerable<object[]> Queries
        {
            get
            {
                yield return new object[] {string.Empty};
                yield return new object[] { "pageSize=10&page=1" };
                yield return new object[] { "filters=name@=r" };
                yield return new object[] { "filters=hp>=100" };
                yield return new object[] { "filters=defense>=100" };
                yield return new object[] { "filters=hp>=100,defense<=200" };
                yield return new object[] { "filters=attack>=100" };
            }
        }


        public static IEnumerable<object[]> QueriesName
        {
            get
            {
                yield return new object[] { "filters=name==Zubat" };
                yield return new object[] { "filters=name@=r" };
            }
        }

        public static IEnumerable<object[]> QueriesGhost
        {
            get
            {
                yield return new object[] { "filters=type1==Ghost" };
                yield return new object[] { "filters=type2==Ghost" };
            }                                             
        }

        public static IEnumerable<object[]> QueriesHp
        {
            get
            {
                yield return new object[] { "filters=type1==Steel" };
                yield return new object[] { "filters=type2==Steel" };
            }
        }

        public static IEnumerable<object[]> QueriesAttack
        {
            get
            {
                yield return new object[] { "filters=type1==Fire" };
                yield return new object[] { "filters=type2==Fire" };
            }
        }

        public static IEnumerable<object[]> QueriesAttackSpeed
        {
            get
            {
                yield return new object[] { "filters=type1@=Bug,type2@=Flying" };
            }
        }

        [Theory]
        [MemberAutoMockData(nameof(Queries))]
        public async Task GetAllPokemonsWithFilter_ShouldReturn_200Result(string queryStr)
        {
            var response = await ((PokemonWebClient) webClient).GetAllPokemons(queryStr);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllPokemonsWithPaginationFilter_ShouldReturn_200Result()
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons("pageSize=10&page=1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().Be(10);
        }

        [Theory]
        [MemberAutoMockData(nameof(QueriesName))]
        public async Task GetAllPokemonsWithFilterName_ShouldReturn_200Result(string queryStr)
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons(queryStr);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllPokemonsParseTestLegendary_ShouldReturn_200ResultZeroRecords()
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons("filters=legendary==true");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAllPokemonsArchiveParseTestLegendary_ShouldReturn_200ResultSomeRecords()
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemonsArchive("filters=legendary==true");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);
        }

        [Theory]
        [MemberAutoMockData(nameof(QueriesGhost))]
        public async Task GetAllPokemonsParseTestGhostType_ShouldReturn_200ResultZeroRecords(string queryStr)
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons(queryStr);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().Be(0);
        }

        [Theory]
        [MemberAutoMockData(nameof(QueriesGhost))]
        public async Task GetAllPokemonsArchiveParseTestGhostType_ShouldReturn_200ResultSomeRecords(string queryStr)
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemonsArchive(queryStr);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();

            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);
        }

        [Theory]
        [MemberAutoMockData(nameof(QueriesHp))]
        public async Task GetAllPokemonsArchiveParseTestSteelType_ShouldReturn_200ResultHpDoubled(string queryStr)
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons(queryStr);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();
            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);

            var resultItem = results.FirstOrDefault();

            var archiveResponse = await ((PokemonWebClient)webClient).GetAllPokemonsArchive($"filters=name=={resultItem.Name}");
            archiveResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var archiveResults = await archiveResponse.GetResultAsync<IEnumerable<PokemonModel>>();
            archiveResults.Should().NotBeNull();
            archiveResults.Count().Should().BeGreaterThan(0);

            var archiveItem = archiveResults.FirstOrDefault();

            resultItem.Should().NotBeNull();
            archiveItem.Should().NotBeNull();

            resultItem.Hp.Should().Be(archiveItem.Hp * 2);
        }


        [Theory]
        [MemberAutoMockData(nameof(QueriesAttack))]
        public async Task GetAllPokemonsArchiveParseTestFireType_ShouldReturn_200ResultAttackLowered(string queryStr)
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons(queryStr);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();
            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);

            var resultItem = results.FirstOrDefault();

            var archiveResponse = await ((PokemonWebClient)webClient).GetAllPokemonsArchive($"filters=name=={resultItem.Name}");
            archiveResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var archiveResults = await archiveResponse.GetResultAsync<IEnumerable<PokemonModel>>();
            archiveResults.Should().NotBeNull();
            archiveResults.Count().Should().BeGreaterThan(0);

            var archiveItem = archiveResults.FirstOrDefault();

            resultItem.Should().NotBeNull();
            archiveItem.Should().NotBeNull();

            resultItem.Attack.Should().Be(archiveItem.Attack * 0.9);
        }

        [Theory]
        [MemberAutoMockData(nameof(QueriesAttackSpeed))]
        public async Task GetAllPokemonsArchiveParseTestBugFlying_ShouldReturn_200ResultAttackSpeedGrowed(string queryStr)
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons(queryStr);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();
            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);

            var resultItem = results.FirstOrDefault();

            var archiveResponse = await ((PokemonWebClient)webClient).GetAllPokemonsArchive($"filters=name=={resultItem.Name}");
            archiveResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var archiveResults = await archiveResponse.GetResultAsync<IEnumerable<PokemonModel>>();
            archiveResults.Should().NotBeNull();
            archiveResults.Count().Should().BeGreaterThan(0);

            var archiveItem = archiveResults.FirstOrDefault();

            resultItem.Should().NotBeNull();
            archiveItem.Should().NotBeNull();

            resultItem.AttackSpeed.Should().Be(archiveItem.AttackSpeed * 1.1);
        }

        [Fact]
        public async Task GetAllPokemonsArchiveParseTestG_ShouldReturn_200ResultAttackDefenseGrowed()
        {
            var response = await ((PokemonWebClient)webClient).GetAllPokemons("filters=name@=G");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var results = await response.GetResultAsync<IEnumerable<PokemonModel>>();
            results.Should().NotBeNull();
            results.Count().Should().BeGreaterThan(0);

            var resultItem = results.FirstOrDefault(x => x.Name.StartsWith("G"));

            var archiveResponse = await ((PokemonWebClient)webClient).GetAllPokemonsArchive($"filters=name=={resultItem.Name}");
            archiveResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var archiveResults = await archiveResponse.GetResultAsync<IEnumerable<PokemonModel>>();
            archiveResults.Should().NotBeNull();
            archiveResults.Count().Should().BeGreaterThan(0);

            var archiveItem = archiveResults.FirstOrDefault();

            resultItem.Should().NotBeNull();
            archiveItem.Should().NotBeNull();

            resultItem.Defense.Should().Be(archiveItem.Defense + 5 * (archiveItem.Name.Length - archiveItem.Name.CountOf("G")));
        }
    }
}
