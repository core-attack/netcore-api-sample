using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace PokemonApi.Common.Extensions
{
    public static class HttpResponseMessageExtentions
    {
        public static async Task<T> GetResultAsync<T>(this HttpResponseMessage result)
        {
            return await result.Content.ReadAsAsync<T>(new[] { new JsonMediaTypeFormatter() }).ConfigureAwait(false);
        }
    }
}
