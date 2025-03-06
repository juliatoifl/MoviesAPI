using Dapper;
using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public class MoviesRepository: IMoviesRepository
{
    
    private readonly string _connectionString;
    public MoviesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<int> Create(Movie movie)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
                        INSERT INTO Movie (Title) 
                        VALUES (@Title);
                        
                        SELECT SCOPE_IDENTITY();
                        ";
            
            var id = await connection.QuerySingleAsync<int>(query, movie);
            movie.Id = id;
            return id;
        }
    }

    public async Task<List<Movie>> GetAll()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var movies = await connection.QueryAsync<Movie>(@"SELECT Id, Title FROM Movie");
            return movies.ToList();
        }
    }

    public async Task<Movie?> GetById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var movie = await connection.QuerySingleOrDefaultAsync<Movie>(@"SELECT Id, Title FROM Movie WHERE Id = @Id", new { id });
            return movie;
        }
    }

    public async Task<bool> Exists(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var exists = await connection.QuerySingleAsync<bool>(@"IF EXISTS (SELECT 1 FROM Movie WHERE Id = @Id)
                                                                        SELECT 1;
                                                                        ELSE SELECT 0;", new { id });
            return exists;
        }
    }
    
    public async Task<int> EnsureMovieExists(SqlConnection connection, SqlTransaction transaction, string title, int releaseYear, int genreId)
    {
        var query = @"
                IF NOT EXISTS (SELECT 1 FROM Movie WHERE Title = @Title AND ReleaseYear = @ReleaseYear)
                BEGIN
                    INSERT INTO Movie (Title, ReleaseYear, GenreId) VALUES (@Title, @ReleaseYear, @GenreId);
                END;
                SELECT Id FROM Movie WHERE Title = @Title AND ReleaseYear = @ReleaseYear;";

        return await connection.ExecuteScalarAsync<int>(query, new { Title = title, ReleaseYear = releaseYear, GenreId = genreId }, transaction);
    }

    public async Task Update(Movie movie)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(@"UPDATE Movie SET Title = @Title WHERE Id = @Id", movie);
        }
    }

    public async Task Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(@"DELETE FROM Movie WHERE Id = @Id", new { id });
        }
    }
    public async Task<Movie?> FindByTitleAndYear(SqlConnection connection, SqlTransaction transaction, string title, int releaseYear)
    {
        const string query = "SELECT Id, Title, ReleaseYear, GenreId FROM Movie WHERE Title = @Title AND ReleaseYear = @ReleaseYear";
        return await connection.QuerySingleOrDefaultAsync<Movie>(query, new { Title = title, ReleaseYear = releaseYear }, transaction);
    }

    public async Task<int> Create(SqlConnection connection, SqlTransaction transaction, Movie movie)
    {
        const string query = @"
                INSERT INTO Movie (Title, ReleaseYear, GenreId) 
                VALUES (@Title, @ReleaseYear, @GenreId);
                SELECT SCOPE_IDENTITY();";

        return await connection.ExecuteScalarAsync<int>(query, movie, transaction);
    }
}