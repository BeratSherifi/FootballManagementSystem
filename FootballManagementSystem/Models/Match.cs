namespace FootballManagementSystem.Models;

public class Match
{
    public int Id { get; set; }
    public int HomeClubId { get; set; }
    public Club HomeClub { get; set; }
    public int AwayClubId { get; set; }
    public Club AwayClub { get; set; }
    public DateTime Date { get; set; }
    public int HomeClubGoals { get; set; }
    public int AwayClubGoals { get; set; }
    public List<Event> Events { get; set; } = new List<Event>();}
