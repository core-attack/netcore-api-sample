using System.Text.RegularExpressions;

namespace PokemonApi.Common.Extensions
{
    public static class StringExtensions
    {
        public static int CountOf(this string s, string sub)
        {
            return Regex.Matches(s.ToUpperInvariant(), sub.ToUpperInvariant()).Count;
        }
    }
}
