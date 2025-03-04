using Microsoft.AspNetCore.Http.HttpResults;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.UseCases;

namespace MoviesAPI.Endpoints;

public static class AcademyAwardsEndpoints
{
    public static RouteGroupBuilder MapAcademyAwards(this RouteGroupBuilder group)
    {
        group.MapPost("/nomination", CreateAcademyAwardNomination);
        group.MapPost("/scandal", ReactToScandalEndpoint);
        group.MapPost("/bulk-import-past-winners", BulkImportPastWinners);
        return group;
    }

    static async Task<IResult> CreateAcademyAwardNomination(
        CreateAcademyAwardNominationDto nominationDto, 
        CreateAcademyAwardNomination useCase)
    {
        var nomination = new AcademyAwardNomination
        {
            ActorFirstName = nominationDto.ActorFirstName,
            ActorLastName = nominationDto.ActorLastName,
            ActorDateOfBirth = nominationDto.ActorDateOfBirth,
            MovieTitle = nominationDto.MovieTitle,
            ReleaseYear = nominationDto.ReleaseYear,
            Genre = nominationDto.Genre,
            Year = nominationDto.Year,
            Category = nominationDto.Category,
            Won = nominationDto.Won
        };

        await useCase.ExecuteAsync(nomination);
        return TypedResults.NoContent();
    }
    
    static async Task<Results<NoContent, NotFound>> ReactToScandalEndpoint(
        ReactToScandal useCase,
        ReactToScandalDto requestDto)
    {
        try
        {
            await useCase.ExecuteAsync(requestDto.ActorId, requestDto.Reason);
            return TypedResults.NoContent();
        }
        catch (Exception)
        {
            return TypedResults.NotFound();
        }
    }
    
    static async Task<IResult> BulkImportPastWinners(
        List<BulkImportPastWinnersDto> pastWinnersDto,
        BulkImportPastWinners useCase)
    {
        var pastWinners = pastWinnersDto.Select(dto => new AcademyAwardNomination
        {
            ActorId = dto.ActorId,
            MovieId = dto.MovieId,
            Year = dto.Year,
            Category = dto.Category,
            Won = true
        }).ToList();

        await useCase.ExecuteAsync(pastWinners);
        return TypedResults.NoContent();
    }
}