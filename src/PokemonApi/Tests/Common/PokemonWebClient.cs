using System.Net.Http;
using System.Threading.Tasks;

namespace Tests.Common
{
    public class PokemonWebClient
    {
        public PokemonWebClient(ApiHostFactory factory, HttpClient client)
        {
            Factory = factory;
            Client = client;
        }

        public ApiHostFactory Factory { get; }

        public HttpClient Client { get; }

        public async Task<HttpResponseMessage> GetAllPokemons(string query)
        {
            return await Client.GetAsync($"/pokemon?{query}");
        }

        public async Task<HttpResponseMessage> GetAllPokemonsArchive(string query)
        {
            return await Client.GetAsync($"/pokemon/archive?{query}");
        }
    }
}
