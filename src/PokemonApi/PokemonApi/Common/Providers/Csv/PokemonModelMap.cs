using CsvHelper.Configuration;
using PokemonApi.Models;

namespace PokemonApi.Common.Providers.Csv
{
    public class PokemonModelMap: ClassMap<PokemonModel>
        {
        public PokemonModelMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Type1).Name("Type 1");
            Map(m => m.Type2).Name("Type 2");
            Map(m => m.Total).Name("Total");
            Map(m => m.Hp).Name("HP");
            Map(m => m.Attack).Name("Attack");
            Map(m => m.Defense).Name("Defense");
            Map(m => m.AttackSpeed).Name("Sp. Atk");
            Map(m => m.DefenseSpeed).Name("Sp. Def");
            Map(m => m.Speed).Name("Speed");
            Map(m => m.Generation).Name("Generation");
            Map(m => m.Legendary).Name("Legendary");
        }
    }
}
