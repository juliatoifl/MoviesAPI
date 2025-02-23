using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Utilities;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Genre, GenreDto>();
        CreateMap<CreateGenreDto, Genre>();
        CreateMap<UpdateGenreDto, Genre>();
        CreateMap<Actor, ActorDto>();
        CreateMap<CreateActorDto, Actor>();
        CreateMap<UpdateActorDto, Actor>();
    }
}