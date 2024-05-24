using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FootballManagementSystem.Models;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Stadium { get; set; }
    
    
    [JsonIgnore] 
    public ICollection<Player> Players { get; set; } = new List<Player>();
    
    
}

