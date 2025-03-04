using Microsoft.Data.SqlClient;
using MoviesAPI.Repositories;

namespace MoviesAPI.UseCases;

public class ReactToScandal
{
    private readonly IScandalLogRepository _scandalLogRepository;
    private readonly IAcademyAwardNominationRepository _nominationRepository;
    private readonly IActorMovieRepository _actorMovieRepository;
    private readonly IActorsRepository _actorRepository;
    private readonly string _connectionString;


    public ReactToScandal(
        IConfiguration configuration,
        IScandalLogRepository scandalLogRepository,
        IAcademyAwardNominationRepository nominationRepository,
        IActorMovieRepository actorMovieRepository,
        IActorsRepository actorRepository)
    {
        _scandalLogRepository = scandalLogRepository;
        _nominationRepository = nominationRepository;
        _actorMovieRepository = actorMovieRepository;
        _actorRepository = actorRepository;
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task ExecuteAsync(int actorId, string reason)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var actor = await _actorRepository.GetById(connection, transaction, actorId);
                    if (actor == null)
                    {
                        throw new Exception("Actor not found.");
                    }

                    await _scandalLogRepository.LogScandal(connection, transaction, actor.FirstName, actor.LastName, reason);
                    await _nominationRepository.DeleteNominationsByActor(connection, transaction, actorId);
                    await _actorMovieRepository.Delete(connection, transaction, actorId);
                    await _actorRepository.Delete(connection, transaction, actorId);
                    
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Transaction failed: {ex.Message}");
                    throw;
                }
            }
        }
    }
}