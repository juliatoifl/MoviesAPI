using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public interface IActorsRepository
{
    Task<int> Create(Actor actor);
    Task<List<Actor>> GetAll();
    Task<Actor?> GetById(int id);
    Task<bool> Exists(int id);
    Task Update(Actor genre);
    Task Delete(int id);
}