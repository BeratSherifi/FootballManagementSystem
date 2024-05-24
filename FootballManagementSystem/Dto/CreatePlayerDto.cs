namespace FootballManagementSystem.Dto;

public class CreatePlayerDto
{
    public string Name { get; set; }
    public string Position { get; set; }
    public int ClubId { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Appearances { get; set; }
}