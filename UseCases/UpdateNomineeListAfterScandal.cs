using Dapper;
using Microsoft.Data.SqlClient;

namespace MoviesAPI.UseCases;

public class UpdateNomineeListAfterScandal
{
    // Transactionality
    
    private readonly string _connectionString;

    public UpdateNomineeListAfterScandal(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task ExecuteAsync(int actorId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    string deleteNominationQuery = @"
                            DELETE FROM AcademyAwardNomination WHERE ActorId = @ActorId;
                        ";
                    await connection.ExecuteAsync(deleteNominationQuery, new { ActorId = actorId }, transaction);

                    string logScandalQuery = @"
                            INSERT INTO ScandalLog (ActorId, Date, Reason) VALUES (@ActorId, GETDATE(), 'Nomination removed due to controversy');
                        ";
                    await connection.ExecuteAsync(logScandalQuery, new { ActorId = actorId }, transaction);

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