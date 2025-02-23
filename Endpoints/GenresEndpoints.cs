using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Repositories;

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
    
    static async Task<Ok<List<GenreDto>>> GetGenres(IGenresRepository genresRepository, IMapper mapper)
    {
        var genres = await genresRepository.GetAll();
        var genreDTOs = mapper.Map<List<GenreDto>>(genres);        
        return TypedResults.Ok(genreDTOs);
    }

    static async Task<Results<Ok<GenreDto>, NotFound>> GetById(int id, IGenresRepository genresRepository, IMapper mapper)
    {
        var genre = await genresRepository.GetById(id);
        if (genre is null)
        {
            return TypedResults.NotFound();
        }

        var genreDTO = mapper.Map<GenreDto>(genre);
        
        return TypedResults.Ok(genreDTO);
    }

    static async Task<Created<GenreDto>> Create(CreateGenreDto createGenreDto, IGenresRepository genresRepository, IMapper mapper)
    {
        var genre = new Genre()
        {
            Name = createGenreDto.Name
        };
        
        var id = await genresRepository.Create(genre);
        
        var genreDTO = mapper.Map<GenreDto>(genre);
        
        return TypedResults.Created(new Uri($"/genres/{id}"), genreDTO);
    }

    static async Task<Results<NotFound, NoContent>> Update(int id, UpdateGenreDto updateGenreDto, IGenresRepository genresRepository, IMapper mapper)
    {
        var exists = await genresRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }

        var genre = mapper.Map<Genre>(updateGenreDto);
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