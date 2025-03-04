using Microsoft.Data.SqlClient;

namespace MoviesAPI.Repositories;

public interface IActorMovieRepository
{
    Task EnsureActorMovieRelationshipExists(SqlConnection connection, SqlTransaction transaction, int actorId, int movieId);
    Task Delete(SqlConnection connection, SqlTransaction transaction, int actorId);

}
