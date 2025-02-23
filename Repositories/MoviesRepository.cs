using Dapper;
using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public class MoviesRepository: IMoviesRepository
{
    
    private readonly string connectionString;
    public MoviesRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<int> Create(Movie movie)
    {
        using (var connection = new SqlConnection(connectionString))
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
        using (var connection = new SqlConnection(connectionString))
        {
            var movies = await connection.QueryAsync<Movie>(@"SELECT Id, Title FROM Movie");
            return movies.ToList();
        }
    }

    public async Task<Movie?> GetById(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var movie = await connection.QuerySingleOrDefaultAsync<Movie>(@"SELECT Id, Title FROM Movie WHERE Id = @Id", new { id });
            return movie;
        }
    }

    public async Task<bool> Exists(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var exists = await connection.QuerySingleAsync<bool>(@"IF EXISTS (SELECT 1 FROM Movie WHERE Id = @Id)
                                                                        SELECT 1;
                                                                        ELSE SELECT 0;", new { id });
            return exists;
        }
    }

    public async Task Update(Movie movie)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(@"UPDATE Movie SET Title = @Title WHERE Id = @Id", movie);
        }
    }

    public async Task Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(@"DELETE FROM Movie WHERE Id = @Id", new { id });
        }
    }
}