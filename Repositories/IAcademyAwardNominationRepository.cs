using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories
{
    public interface IAcademyAwardNominationRepository
    {
        Task CreateNomination(SqlConnection connection, SqlTransaction transaction, int actorId, int movieId, int year, string category, bool won);
        Task<List<AcademyAwardNomination>> GetNominations();
    }
}