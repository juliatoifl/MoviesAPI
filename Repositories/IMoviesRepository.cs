using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public interface IMoviesRepository
{
    Task<int> Create(Movie movie);
    Task<List<Movie>> GetAll();
    Task<Movie?> GetById(int id);
    Task<bool> Exists(int id);
    Task Update(Movie movie);
    Task Delete(int id);
}