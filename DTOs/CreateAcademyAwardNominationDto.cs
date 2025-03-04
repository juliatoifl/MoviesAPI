namespace MoviesAPI.DTOs
{
    public class CreateAcademyAwardNominationDto
    {
        public string ActorFirstName { get; set; } = null!;
        public string ActorLastName { get; set; } = null!;
        public DateTime ActorDateOfBirth { get; set; }
        public string MovieTitle { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public string Genre { get; set; } = null!;
        public int Year { get; set; }
        public string Category { get; set; } = null!;
        public bool Won { get; set; }
    }
}