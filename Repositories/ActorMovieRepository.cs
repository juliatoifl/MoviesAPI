using Dapper;
using Microsoft.Data.SqlClient;

namespace MoviesAPI.Repositories
{
    public class ActorMovieRepository : IActorMovieRepository
    {
        public async Task EnsureActorMovieRelationshipExists(SqlConnection connection, SqlTransaction transaction, int actorId, int movieId)
        {
            var query = @"
                IF NOT EXISTS (SELECT 1 FROM ActorMovie WHERE ActorId = @ActorId AND MovieId = @MovieId)
                BEGIN
                    INSERT INTO ActorMovie (ActorId, MovieId) VALUES (@ActorId, @MovieId);
                END;";

            await connection.ExecuteAsync(query, new { ActorId = actorId, MovieId = movieId }, transaction);
        }
        
        public async Task Delete(SqlConnection connection, SqlTransaction transaction, int actorId)
        {
            var query = "DELETE FROM ActorMovie WHERE ActorId = @ActorId";
            await connection.ExecuteAsync(query, new { ActorId = actorId }, transaction);
        }
    }
}