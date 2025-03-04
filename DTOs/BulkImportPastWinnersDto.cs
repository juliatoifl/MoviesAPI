namespace MoviesAPI.DTOs
{
    public class BulkImportPastWinnersDto
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public int Year { get; set; }
        public string Category { get; set; } = null!;
    }
}