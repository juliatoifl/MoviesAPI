namespace MoviesAPI.Entities
{
    public class AcademyAwardNomination
    {
        public int Id { get; set; }
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public int Year { get; set; }
        public string Category { get; set; } = null!;
        public bool Won { get; set; }

        public Actor? Actor { get; set; }
        public Movie? Movie { get; set; }

        public string ActorFirstName { get; set; } = null!;
        public string ActorLastName { get; set; } = null!;
        public DateTime ActorDateOfBirth { get; set; }
        public string MovieTitle { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public string Genre { get; set; } = null!;
    }
}