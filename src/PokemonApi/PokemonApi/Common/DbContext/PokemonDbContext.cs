using System;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Models;

namespace PokemonApi.Common.DbContext
{
    public class PokemonDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PokemonDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PokemonTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PokemonArchiveTypeConfiguration());
        }
    }
}
