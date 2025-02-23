using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Repositories;

namespace MoviesAPI.Endpoints;

public static class ActorsEndpoints
{
    public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetActors);
        group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);
        return group;
    }

    static async Task<Ok<List<ActorDto>>> GetActors(IActorsRepository actorsRepository, IMapper mapper)
    {
        var actors = await actorsRepository.GetAll();
        var actorsDto = mapper.Map<List<ActorDto>>(actors);
        return TypedResults.Ok(actorsDto);
    }

    static async Task<Results<Ok<ActorDto>, NotFound>> GetById(int id, IActorsRepository actorsRepository, IMapper mapper)
    {
        var actor = await actorsRepository.GetById(id);
        if (actor is null)
            return TypedResults.NotFound();
        
        var actorDto = mapper.Map<ActorDto>(actor);
        return TypedResults.Ok(actorDto);
    }
    
    static async Task<Created<ActorDto>> Create(CreateActorDto createActorDto, IActorsRepository actorsRepository, IMapper mapper)
    {
        var actor = new Actor
        {
            FirstName = createActorDto.FirstName,
            LastName = createActorDto.LastName,
            DateOfBirth = createActorDto.DateOfBirth
        };

        var id = await actorsRepository.Create(actor);
        actor.Id = id;

        var actorDto = mapper.Map<ActorDto>(actor);
        return TypedResults.Created($"/actors/{id}", actorDto);
    }

    static async Task<Results<NoContent, NotFound>> Update(int id, UpdateActorDto updateActorDto, IActorsRepository actorsRepository, IMapper mapper)
    {
        var exists = await actorsRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }

        var actor = mapper.Map<Actor>(updateActorDto);
        actor.Id = id;
    
        await actorsRepository.Update(actor);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> Delete(int id, IActorsRepository actorsRepository)
    {
        var exists = await actorsRepository.Exists(id);
        if (!exists)
        {
            return TypedResults.NotFound();
        }

        await actorsRepository.Delete(id);
        return TypedResults.NoContent();
    }
}
