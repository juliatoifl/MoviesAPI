using Microsoft.Data.SqlClient;
using MoviesAPI.Entities;

namespace MoviesAPI.Repositories;

public interface IMoviesRepository
{
    Task<int> Create(Movie movie);
    Task<List<Movie>> GetAll();
    Task<Movie?> GetById(int id);
    Task<bool> Exists(int id);
    Task<int> EnsureMovieExists(SqlConnection connection, SqlTransaction transaction, string title, int releaseYear, int genreId);    
    Task Update(Movie movie);
    Task Delete(int id);
    Task<Movie?> FindByTitleAndYear(SqlConnection connection, SqlTransaction transaction, string title, int releaseYear);
    Task<int> Create(SqlConnection connection, SqlTransaction transaction, Movie movie);

}
