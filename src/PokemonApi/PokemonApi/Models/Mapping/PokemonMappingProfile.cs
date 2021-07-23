using AutoMapper;

namespace PokemonApi.Models.Mapping
{
    public class PokemonMappingProfile : Profile
    {
        public PokemonMappingProfile()
        {
            CreateMap<Pokemon, PokemonModel>();
            CreateMap<PokemonArchive, PokemonModel>();
            CreateMap<PokemonModel, Pokemon>();
            CreateMap<PokemonModel, PokemonArchive>();
        }
    }
}
