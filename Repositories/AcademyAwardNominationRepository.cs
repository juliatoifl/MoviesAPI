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

        public async Task<List<AcademyAwardNomination>> GetNominations()
        {
            using (var connection = new SqlConnection("YourConnectionStringHere"))
            {
                var query = @"
                    SELECT aan.Id, aan.ActorId, aan.MovieId, aan.Year, aan.Category, aan.Won,
                           a.FirstName AS ActorFirstName, a.LastName AS ActorLastName, a.DateOfBirth AS ActorDateOfBirth,
                           m.Title AS MovieTitle, m.ReleaseYear,
                           g.Name AS Genre
                    FROM AcademyAwardNomination aan
                    INNER JOIN Actor a ON aan.ActorId = a.Id
                    INNER JOIN Movie m ON aan.MovieId = m.Id
                    INNER JOIN Genre g ON m.GenreId = g.Id;";

                var nominations = await connection.QueryAsync<AcademyAwardNomination>(query);
                return nominations.AsList();
            }
        }
    }
}