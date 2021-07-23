using Newtonsoft.Json;
using Sieve.Attributes;

namespace PokemonApi.Models
{
    public class PokemonModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type1")]
        public string Type1 { get; set; }

        [JsonProperty("type2")]
        public string Type2 { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("hp")]
        public int Hp { get; set; }

        [JsonProperty("attack")]
        public double Attack { get; set; }

        [JsonProperty("defense")]
        public double Defense { get; set; }

        [JsonProperty("attackSpeed")]
        public double AttackSpeed { get; set; }

        [JsonProperty("defenseSpeed")]
        public double DefenseSpeed { get; set; }

        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("generation")]
        public int Generation { get; set; }

        [JsonProperty("legendary")]
        public bool Legendary { get; set; }
    }
}
