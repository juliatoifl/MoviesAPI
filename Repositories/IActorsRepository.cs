using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public interface IActorsRepository
{
    Task<int> Create(Actor actor);
    Task<List<Actor>> GetAll();
    Task<Actor?> GetById(int id);
    Task<bool> Exists(int id);
    Task<int> EnsureActorExists(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName, DateTime dateOfBirth);    Task Update(Actor genre);
    Task Delete(int id);
}