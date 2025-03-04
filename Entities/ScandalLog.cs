namespace MoviesAPI.Entities
{
    public class ScandalLog
    {
        public int Id { get; set; }
        public int ActorId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; } = null!;

        // Navigation property
        public Actor? Actor { get; set; }
    }
}