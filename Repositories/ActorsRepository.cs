using Dapper;
using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public class ActorsRepository: IActorsRepository
{
    private readonly string connectionString;

    public ActorsRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> Create(Actor actor)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var query = @"
                        INSERT INTO Actor (FirstName, LastName, DateOfBirth) 
                        VALUES (@FirstName, @LastName, @DateOfBirth);
                        
                        SELECT SCOPE_IDENTITY();
                        ";
            
            var id = await connection.ExecuteScalarAsync<int>(query, actor);
            
            actor.Id = id;
            return id;
        }
    }

    public async Task<List<Actor>> GetAll()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var actors = await connection.QueryAsync<Actor>(@"SELECT Id, FirstName, LastName, DateOfBirth FROM Actor");
            return actors.ToList();
        }
    }

    public async Task<Actor?> GetById(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var actor = await connection.QuerySingleOrDefaultAsync<Actor>(@"SELECT Id, FirstName, LastName, DateOfBirth FROM Actor WHERE Id = @Id", new { id });
            return actor;
        }
    }

    public async Task<bool> Exists(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var exists = await connection.QuerySingleAsync<bool>(@"IF EXISTS (SELECT 1 FROM Actor WHERE Id = @Id)
                                                                        SELECT 1;
                                                                        ELSE SELECT 0;", new { id });
            return exists;
        }
    }

    public async Task Update(Actor actor)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(@"UPDATE Actor SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth WHERE Id = @Id", actor);
        }
    }

    public async Task Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(@"DELETE FROM Actor WHERE Id = @Id", new { id });
        }
    }
}