// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonApi.Common.DbContext;

namespace PokemonApi.Migrations
{
    [DbContext(typeof(PokemonDbContext))]
    [Migration("20210618135953_AddPokemonArchive")]
    partial class AddPokemonArchive
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("PokemonApi.Models.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Attack")
                        .HasColumnType("double")
                        .HasDefaultValue(0.0);

                    b.Property<double>("AttackSpeed")
                        .HasColumnType("double")
                        .HasDefaultValue(0.0);

                    b.Property<double>("Defense")
                        .HasColumnType("double")
                        .HasDefaultValue(0.0);

                    b.Property<double>("DefenseSpeed")
                        .HasColumnType("double")
                        .HasDefaultValue(0.0);

                    b.Property<int>("Generation")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Hp")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("Legendary")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Speed")
                        .HasColumnType("double")
                        .HasDefaultValue(0.0);

                    b.Property<int>("Total")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Type1")
                        .HasColumnType("text");

                    b.Property<string>("Type2")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Pokemons");
                });
#pragma warning restore 612, 618
        }
    }
}
