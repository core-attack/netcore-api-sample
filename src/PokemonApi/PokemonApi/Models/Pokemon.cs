using System;
using Sieve.Attributes;

namespace PokemonApi.Models
{
    public class Pokemon
    {
        public Pokemon(int id, string name, string type1, string type2, int total, int hp, double attack,
            double defense, double spAttack, double spDefense, double speed, int generation, bool legendary)
        {
            Id = id;
            SetName(name);
            Type1 = type1;
            Type2 = type2;
            Total = total;
            Hp = hp;
            Attack = attack;
            Defense = defense;
            AttackSpeed = spAttack;
            DefenseSpeed = spDefense;
            Speed = speed;
            Generation = generation;
            Legendary = legendary;
        }

        private Pokemon() { }

        [Sieve(CanFilter = true, Name = "id")]
        public int Id { get; private set; }
        
        [Sieve(CanFilter = true, Name = "name")]
        public string Name { get; private set; }

        [Sieve(CanFilter = true, Name = "type1")]
        public string Type1 { get; private set; }

        [Sieve(CanFilter = true, Name = "type2")]
        public string Type2 { get; private set; }

        [Sieve(CanFilter = true, Name = "total")]
        public int Total { get; private set; }

        [Sieve(CanFilter = true, Name = "hp")]
        public int Hp { get; private set; }

        [Sieve(CanFilter = true, Name = "attack")]
        public double Attack { get; private set; }

        [Sieve(CanFilter = true, Name = "defense")]
        public double Defense { get; private set; }

        [Sieve(CanFilter = true, Name = "attackSpeed")]
        public double AttackSpeed { get; private set; }

        [Sieve(CanFilter = true, Name = "defenseSpeed")]
        public double DefenseSpeed { get; private set; }

        [Sieve(CanFilter = true, Name = "speed")]
        public double Speed { get; private set; }

        [Sieve(CanFilter = true, Name = "generation")]
        public int Generation { get; private set; }

        [Sieve(CanFilter = true, Name = "legendary")]
        public bool Legendary { get; private set; }

        private void SetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Name = value;
        }
    }
}
