namespace FootballManagementSystem.Models;

public class Event
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public Match Match { get; set; }
    public int PlayerId { get; set; }
    public Player Player { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
}
