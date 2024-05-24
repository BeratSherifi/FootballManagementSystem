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

        // Configure the relationships and other settings

        modelBuilder.Entity<Club>()
            .HasMany(c => c.Players)
            .WithOne(p => p.Club)
            .HasForeignKey(p => p.ClubId);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeClub)
            .WithMany()
            .HasForeignKey(m => m.HomeClubId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayClub)
            .WithMany()
            .HasForeignKey(m => m.AwayClubId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Match)
            .WithMany(m => m.Events)
            .HasForeignKey(e => e.MatchId);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Player)
            .WithMany()
            .HasForeignKey(e => e.PlayerId);
    }
}