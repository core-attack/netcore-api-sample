using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonApi.Models
{
    public class PokemonTypeConfiguration : IEntityTypeConfiguration<Pokemon>
    {
        public void Configure(EntityTypeBuilder<Pokemon> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Pokemons");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id)
                .IsUnique();

            builder.Property(c => c.Name)
                .IsRequired(true);

            builder.Property(c => c.Type1);
            builder.Property(c => c.Type2);
            builder.Property(c => c.Total).HasDefaultValue(0);
            builder.Property(c => c.Hp).HasDefaultValue(0);
            builder.Property(c => c.Attack).HasDefaultValue(0);
            builder.Property(c => c.Defense).HasDefaultValue(0);
            builder.Property(c => c.AttackSpeed).HasDefaultValue(0);
            builder.Property(c => c.DefenseSpeed).HasDefaultValue(0);
            builder.Property(c => c.Speed).HasDefaultValue(0);
            builder.Property(c => c.Generation).HasDefaultValue(0);
            builder.Property(c => c.Legendary).HasDefaultValue(false);
        }
    }
}
