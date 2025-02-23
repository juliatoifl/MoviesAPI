using MoviesAPI.Entities;

namespace MoviesAPI.Genres;

public interface IGenresRepository
{
    Task<int> Create(Genre genre);
    Task<List<Genre>> GetAll();
    Task<Genre?> GetById(int id);
    Task<bool> Exists(int id);
    Task Update(Genre genre);
    Task Delete(int id);
    
}