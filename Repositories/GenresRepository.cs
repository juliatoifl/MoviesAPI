using Dapper;
using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public class GenresRepository : IGenresRepository
{
    private readonly string _connectionString;
    public GenresRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<int> Create(Genre genre)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
                        INSERT INTO Genre (Name) 
                        VALUES (@Name);
                        
                        SELECT SCOPE_IDENTITY();
                        ";
            
            var id = await connection.QuerySingleAsync<int>(query, genre);
            genre.Id = id;
            return id;
        }
    }

    public async Task<List<Genre>> GetAll()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var genres = await connection.QueryAsync<Genre>(@"SELECT Id, Name FROM Genre");
            return genres.ToList();
        }
    }

    public async Task<Genre?> GetById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var genre = await connection.QuerySingleOrDefaultAsync<Genre>(@"SELECT Id, Name FROM Genre WHERE Id = @Id", new { id });
            return genre;
        }
    }

    public async Task<bool> Exists(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var exists = await connection.QuerySingleAsync<bool>(@"IF EXISTS (SELECT 1 FROM Genre WHERE Id = @Id)
                                                                        SELECT 1;
                                                                        ELSE SELECT 0;", new { id });
            return exists;
        }
    }
    
        public async Task<int> EnsureGenreExists(SqlConnection connection, SqlTransaction transaction, string genreName)
        {
            var query = @"
                IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = @GenreName)
                BEGIN
                    INSERT INTO Genre (Name) VALUES (@GenreName);
                END;
                SELECT Id FROM Genre WHERE Name = @GenreName;";

            return await connection.ExecuteScalarAsync<int>(query, new { GenreName = genreName }, transaction);
        }

    public async Task Update(Genre genre)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(@"UPDATE Genre SET Name = @Name WHERE Id = @Id", genre);
        }
    }

    public async Task Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(@"DELETE FROM Genre WHERE Id = @Id", new { id });
        }
    }
    
    public async Task<Genre?> FindByName(SqlConnection connection, SqlTransaction transaction, string name)
    {
        const string query = "SELECT Id, Name FROM Genre WHERE Name = @Name";
        return await connection.QuerySingleOrDefaultAsync<Genre>(query, new { Name = name }, transaction);
    }

    public async Task<int> Insert(SqlConnection connection, SqlTransaction transaction, Genre genre)
    {
        const string query = @"
                INSERT INTO Genre (Name) 
                VALUES (@Name);
                SELECT SCOPE_IDENTITY();";

        return await connection.ExecuteScalarAsync<int>(query, genre, transaction);
    }
}