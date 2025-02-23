using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Repositories;

namespace MoviesAPI.Endpoints;

public static class MoviesEndpoints
{
    public static RouteGroupBuilder MapMovies(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetMovies);
        group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);
        return group;
    }   
    
    static async Task<Ok<List<MovieDto>>> GetMovies(IMoviesRepository moviesRepository, IMapper mapper)
    {
        var movies = await moviesRepository.GetAll();
        var movieDTOs = mapper.Map<List<MovieDto>>(movies);        
        return TypedResults.Ok(movieDTOs);
    }

    static async Task<Results<Ok<MovieDto>, NotFound>> GetById(int id, IMoviesRepository moviesRepository, IMapper mapper)
    {
        var movie = await moviesRepository.GetById(id);
        if (movie is null)
        {
            return TypedResults.NotFound();
        }

        var movieDTO = mapper.Map<MovieDto>(movie);
        
        return TypedResults.Ok(movieDTO);
    }

    static async Task<Created<MovieDto>> Create(CreateMovieDto createMovieDto, IMoviesRepository moviesRepository, IMapper mapper)
    {
        var movie = new Movie()
        {
            Title = createMovieDto.Title
        };
        
        var id = await moviesRepository.Create(movie);
        
        var movieDTO = mapper.Map<MovieDto>(movie);
        
        return TypedResults.Created(new Uri($"/movies/{id}"), movieDTO);
    }

    static async Task<Results<NotFound, NoContent>> Update(int id, UpdateMovieDto updateMovieDto, IMoviesRepository moviesRepository, IMapper mapper)
    {
        var exists = await moviesRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }

        var movie = mapper.Map<Movie>(updateMovieDto);
        movie.Id = id;
    
        await moviesRepository.Update(movie);
        return TypedResults.NoContent();
    }

    static async Task<Results<NotFound, NoContent>> Delete(int id, IMoviesRepository moviesRepository)
    {
        var exists = await moviesRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }
    
        await moviesRepository.Delete(id);
        return TypedResults.NoContent();
    }
}