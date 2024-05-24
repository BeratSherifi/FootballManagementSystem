using FootballManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace FootballManagementSystem;
public class FootballContext : DbContext
{
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Event> Events { get; set; }

    public FootballContext(DbContextOptions<FootballContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Define relationships and constraints
    }
}