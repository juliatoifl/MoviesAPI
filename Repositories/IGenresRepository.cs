using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public interface IGenresRepository
{
    Task<int> Create(Genre genre);
    Task<List<Genre>> GetAll();
    Task<Genre?> GetById(int id);
    Task<Genre?> FindByName(SqlConnection connection, SqlTransaction transaction, string name);
    Task<int> Insert(SqlConnection connection, SqlTransaction transaction, Genre genre);
    Task<bool> Exists(int id);
    Task<int> EnsureGenreExists(SqlConnection connection, SqlTransaction transaction, string genreName);
    Task Update(Genre genre);
    Task Delete(int id);
    
}
