using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;
using MoviesAPI.Repositories;

namespace MoviesAPI.UseCases;

public class BulkImportPastWinners
{
    private readonly string _connectionString;
    private readonly IAcademyAwardNominationRepository _nominationRepository;

    public BulkImportPastWinners(IConfiguration configuration, IAcademyAwardNominationRepository nominationRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _nominationRepository = nominationRepository;
    }

    public async Task ExecuteAsync(List<AcademyAwardNomination> nominations)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    await _nominationRepository.BulkImportPastWinners(connection, transaction, nominations);
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
