namespace MoviesAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public int GenreId { get; set; }
        
        // Navigation property
        public Genre? Genre { get; set; }
    }
}
