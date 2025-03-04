using Microsoft.AspNetCore.Http.HttpResults;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.UseCases;

namespace MoviesAPI.Endpoints;

public static class AcademyAwardsEndpoints
{
    public static RouteGroupBuilder MapAcademyAwards(this RouteGroupBuilder group)
    {
        group.MapPost("/nominations", CreateAcademyAwardNominations);
        return group;
    }

    static async Task<NoContent> CreateAcademyAwardNominations(
        List<CreateAcademyAwardNominationDto> nominationsDto, 
        CreateAcademyAwardNominations useCase)
    {
        var nominations = nominationsDto.Select(dto => new AcademyAwardNomination
        {
            ActorFirstName = dto.ActorFirstName,
            ActorLastName = dto.ActorLastName,
            ActorDateOfBirth = dto.ActorDateOfBirth,
            MovieTitle = dto.MovieTitle,
            ReleaseYear = dto.ReleaseYear,
            Genre = dto.Genre,
            Year = dto.Year,
            Category = dto.Category,
            Won = dto.Won
        }).ToList();

        await useCase.ExecuteAsync(nominations);
        return TypedResults.NoContent();
    }
}