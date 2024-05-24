using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace FootballManagementSystem.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public int ClubId { get; set; }

    [JsonIgnore] 
    public Club Club { get; set; }

    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Appearances { get; set; }
}