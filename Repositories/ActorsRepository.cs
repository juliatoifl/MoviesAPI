using Dapper;
using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public class ActorsRepository: IActorsRepository
{
    private readonly string _connectionString;

    public ActorsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> Create(Actor actor)
    {
        using (var connection = new SqlConnection(_connectionString))
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
        using (var connection = new SqlConnection(_connectionString))
        {
            var actors = await connection.QueryAsync<Actor>(@"SELECT Id, FirstName, LastName, DateOfBirth FROM Actor");
            return actors.ToList();
        }
    }

    public async Task<Actor?> GetById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var actor = await connection.QuerySingleOrDefaultAsync<Actor>(@"SELECT Id, FirstName, LastName, DateOfBirth FROM Actor WHERE Id = @Id", new { id });
            return actor;
        }
    }
    
    public async Task<Actor?> GetById(SqlConnection connection, SqlTransaction transaction, int actorId)
    {
        var query = "SELECT FirstName, LastName FROM Actor WHERE Id = @ActorId";
        return await connection.QuerySingleOrDefaultAsync<Actor>(query, new { ActorId = actorId }, transaction);
    }

    public async Task<bool> Exists(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var exists = await connection.QuerySingleAsync<bool>(@"IF EXISTS (SELECT 1 FROM Actor WHERE Id = @Id)
                                                                        SELECT 1;
                                                                        ELSE SELECT 0;", new { id });
            return exists;
        }
    }
    
    public async Task<int> EnsureActorExists(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName, DateTime dateOfBirth)
    {
        var query = @"
                IF NOT EXISTS (SELECT 1 FROM Actor WHERE FirstName = @FirstName AND LastName = @LastName)
                BEGIN
                    INSERT INTO Actor (FirstName, LastName, DateOfBirth) 
                    VALUES (@FirstName, @LastName, @DateOfBirth);
                END;
                SELECT Id FROM Actor WHERE FirstName = @FirstName AND LastName = @LastName;";

        return await connection.ExecuteScalarAsync<int>(query, new { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth }, transaction);
    }

    public async Task Update(Actor actor)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(@"UPDATE Actor SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth WHERE Id = @Id", actor);
        }
    }

    public async Task Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(@"DELETE FROM Actor WHERE Id = @Id", new { id });
        }
    }
    
    public async Task Delete(SqlConnection connection, SqlTransaction transaction, int actorId)
    {
        var query = "DELETE FROM Actor WHERE Id = @ActorId";
        await connection.ExecuteAsync(query, new { ActorId = actorId }, transaction);
    }
    
    public async Task<Actor?> FindByDetails(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName, DateTime dateOfBirth)
    {
        const string query = "SELECT Id, FirstName, LastName, DateOfBirth FROM Actor WHERE FirstName = @FirstName AND LastName = @LastName AND DateOfBirth = @DateOfBirth";
        return await connection.QuerySingleOrDefaultAsync<Actor>(query, new { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth }, transaction);
    }

    public async Task<int> Create(SqlConnection connection, SqlTransaction transaction, Actor actor)
    {
        const string query = @"
                INSERT INTO Actor (FirstName, LastName, DateOfBirth) 
                VALUES (@FirstName, @LastName, @DateOfBirth);
                SELECT SCOPE_IDENTITY();";

        return await connection.ExecuteScalarAsync<int>(query, actor, transaction);
    }
}