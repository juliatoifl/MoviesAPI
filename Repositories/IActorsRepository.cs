using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public interface IActorsRepository
{
    Task<int> Create(Actor actor);
    Task<List<Actor>> GetAll();
    Task<Actor?> GetById(int id);
    Task<Actor?> GetById(SqlConnection connection, SqlTransaction transaction, int actorId);
    Task<bool> Exists(int id);
    Task<int> EnsureActorExists(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName, DateTime dateOfBirth);    Task Update(Actor genre);
    Task Delete(int id);
    Task Delete(SqlConnection connection, SqlTransaction transaction, int actorId);
    Task<Actor?> FindByDetails(SqlConnection connection, SqlTransaction transaction, string firstName, string lastName, DateTime dateOfBirth);
    Task<int> Create(SqlConnection connection, SqlTransaction transaction, Actor actor);

}
