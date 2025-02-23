using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Genres;

namespace MoviesAPI.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetGenres);
        group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);
        return group;
    }   
    
    static async Task<Ok<List<GenreDTO>>> GetGenres(IGenresRepository genresRepository, IMapper mapper)
    {
        var genres = await genresRepository.GetAll();
        var genreDTOs = mapper.Map<List<GenreDTO>>(genres);        
        return TypedResults.Ok(genreDTOs);
    }

    static async Task<Results<Ok<GenreDTO>, NotFound>> GetById(int id, IGenresRepository genresRepository, IMapper mapper)
    {
        var genre = await genresRepository.GetById(id);
        if (genre is null)
        {
            return TypedResults.NotFound();
        }

        var genreDTO = mapper.Map<GenreDTO>(genre);
        
        return TypedResults.Ok(genreDTO);
    }

    static async Task<Created<GenreDTO>> Create(CreateGenreDTO createGenreDto, IGenresRepository genresRepository, IMapper mapper)
    {
        var genre = new Genre()
        {
            Name = createGenreDto.Name
        };
        
        var id = await genresRepository.Create(genre);
        
        var genreDTO = mapper.Map<GenreDTO>(genre);
        
        return TypedResults.Created(new Uri($"/genres/{id}"), genreDTO);
    }

    static async Task<Results<NotFound, NoContent>> Update(int id, CreateGenreDTO createGenreDto, IGenresRepository genresRepository, IMapper mapper)
    {
        var exists = await genresRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }

        var genre = mapper.Map<Genre>(createGenreDto);
        genre.Id = id;
    
        await genresRepository.Update(genre);
        return TypedResults.NoContent();
    }

    static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository genresRepository)
    {
        var exists = await genresRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }
    
        await genresRepository.Delete(id);
        return TypedResults.NoContent();
    }
}