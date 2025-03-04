using Microsoft.Data.SqlClient;

namespace MoviesAPI.Repositories
{
    public interface IScandalLogRepository
    {
        Task LogScandal(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName,
            string reason);
    }
}

