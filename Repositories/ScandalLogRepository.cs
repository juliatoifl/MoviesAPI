using Dapper;
using Microsoft.Data.SqlClient;

namespace MoviesAPI.Repositories
{
    public class ScandalLogRepository : IScandalLogRepository
    {
        public async Task LogScandal(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName, string reason)
        {
            var query = @"
            INSERT INTO ScandalLog (ActorFirstName, ActorLastName, Date, Reason, DeletedNominations)
            VALUES (@FirstName, @LastName, GETDATE(), @Reason, 1);";

            await connection.ExecuteAsync(query, new { FirstName = firstName, LastName = lastName, Reason = reason }, transaction);
        }
    }
}
