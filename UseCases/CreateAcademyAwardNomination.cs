using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;
using MoviesAPI.Repositories;

namespace MoviesAPI.UseCases;

public class CreateAcademyAwardNomination
{
    private readonly IMoviesRepository _movieRepository;
    private readonly IActorsRepository _actorRepository;
    private readonly IGenresRepository _genreRepository;
    private readonly IAcademyAwardNominationRepository _nominationRepository;
    private readonly IActorMovieRepository _actorMovieRepository;
    private readonly string _connectionString;

    public CreateAcademyAwardNomination(
        IConfiguration configuration,
        IMoviesRepository movieRepository,
        IActorsRepository actorRepository,
        IGenresRepository genreRepository,
        IAcademyAwardNominationRepository nominationRepository,
        IActorMovieRepository actorMovieRepository)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _genreRepository = genreRepository;
        _nominationRepository = nominationRepository;
        _actorMovieRepository = actorMovieRepository;
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    }
    public async Task ExecuteAsync(AcademyAwardNomination nomination)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    int genreId = await _genreRepository.EnsureGenreExists(connection, transaction, nomination.Genre);
                    int movieId = await _movieRepository.EnsureMovieExists(connection, transaction, nomination.MovieTitle, nomination.ReleaseYear, genreId);
                    int actorId = await _actorRepository.EnsureActorExists(connection, transaction, nomination.ActorFirstName, nomination.ActorLastName, nomination.ActorDateOfBirth);

                    await _actorMovieRepository.EnsureActorMovieRelationshipExists(connection, transaction, actorId, movieId);
                    await _nominationRepository.CreateNomination(connection, transaction, actorId, movieId, nomination.Year, nomination.Category, nomination.Won);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
