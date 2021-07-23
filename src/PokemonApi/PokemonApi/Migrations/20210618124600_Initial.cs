using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PokemonApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type1 = table.Column<string>(type: "text", nullable: true),
                    Type2 = table.Column<string>(type: "text", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Hp = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Attack = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0),
                    Defense = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0),
                    AttackSpeed = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0),
                    DefenseSpeed = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0),
                    Speed = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0),
                    Generation = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Legendary = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_Id",
                table: "Pokemons",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "Pokemons");
        }
    }
}
