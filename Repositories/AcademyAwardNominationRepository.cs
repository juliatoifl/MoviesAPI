using Dapper;
using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories
{
    public class AcademyAwardNominationRepository : IAcademyAwardNominationRepository
    {
        public async Task CreateNomination(SqlConnection connection, SqlTransaction transaction, int actorId, int movieId, int year, string category, bool won)
        {
            var query = @"
                INSERT INTO AcademyAwardNomination (ActorId, MovieId, Year, Category, Won)
                VALUES (@ActorId, @MovieId, @Year, @Category, @Won);";

            await connection.ExecuteAsync(query, new { ActorId = actorId, MovieId = movieId, Year = year, Category = category, Won = won }, transaction);
        }
        
        public async Task DeleteNominationsByActor(SqlConnection connection, SqlTransaction transaction, int actorId)
        {
            var query = "DELETE FROM AcademyAwardNomination WHERE ActorId = @ActorId";
            await connection.ExecuteAsync(query, new { ActorId = actorId }, transaction);
        }
        
        public async Task BulkImportPastWinners(SqlConnection connection, SqlTransaction transaction, List<AcademyAwardNomination> pastWinners)
        {
            var query = @"
            INSERT INTO AcademyAwardNomination (ActorId, MovieId, Year, Category, Won)
            VALUES (@ActorId, @MovieId, @Year, @Category, 1);";

            await connection.ExecuteAsync(query, pastWinners, transaction);
        }
    }
}
