namespace MoviesAPI.DTOs;

public class UpdateActorDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
