namespace MoviesAPI.Entities
{
    public class ScandalLog
    {
        public int Id { get; set; }
        public string ActorFirstName { get; set; } = null!;
        public string ActorLastName { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Reason { get; set; } = null!;
        public bool DeletedNominations { get; set; }
    }
}