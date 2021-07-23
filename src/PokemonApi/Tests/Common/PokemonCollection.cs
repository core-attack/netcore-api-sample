using Xunit;

namespace Tests.Common
{
    [CollectionDefinition("Pokemon Сollection")]
    public class PokemonCollection : ICollectionFixture<PokemonFixture>
    {
    }
}
