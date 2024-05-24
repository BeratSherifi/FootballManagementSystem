using FootballManagementSystem;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class PlayerServiceTests
{
    private DbContextOptions<FootballContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<FootballContext>()
            .UseInMemoryDatabase(databaseName: $"FootballTestDb_{System.Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task CreatePlayer_ShouldAddNewPlayer()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var player = new Player { Name = "New Player", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };

            var result = await service.CreatePlayer(player);

            Assert.NotNull(result);
            Assert.Equal("New Player", result.Name);
            Assert.Equal("Forward", result.Position);
        }

        using (var context = new FootballContext(options))
        {
            Assert.Equal(1, await context.Players.CountAsync());
        }
    }

    [Fact]
    public async Task DeletePlayer_ShouldRemovePlayer()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var player = new Player { Name = "Player to Delete", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.DeletePlayer(1);

            Assert.True(result);
            Assert.Null(await context.Players.FindAsync(1));
        }
    }

    [Fact]
    public async Task DeletePlayer_ShouldReturnFalse_WhenPlayerDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.DeletePlayer(999);

            Assert.False(result);
        }
    }

    [Fact]
    public async Task GetAllPlayers_ShouldReturnAllPlayers()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            // Ensure the clubs exist
            var club = new Club { Name = "Club 1", Stadium = "Stadium 1" };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();

            // Add players
            context.Players.AddRange(
                new Player { Name = "Player 1", Position = "Forward", ClubId = club.Id, Goals = 10, Assists = 5, Appearances = 20 },
                new Player { Name = "Player 2", Position = "Midfielder", ClubId = club.Id, Goals = 5, Assists = 7, Appearances = 15 });
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.GetAllPlayers();

            Assert.Equal(2, result.Count());
        }
    }


    [Fact]
    public async Task GetPlayerById_ShouldReturnPlayer_WhenPlayerExists()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            // Ensure the club exists
            var club = new Club { Name = "Club 1", Stadium = "Stadium 1" };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();

            // Add player
            var player = new Player { Name = "Player 1", Position = "Forward", ClubId = club.Id, Goals = 10, Assists = 5, Appearances = 20 };
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.GetPlayerById(1);

            Assert.NotNull(result);
            Assert.Equal("Player 1", result.Name);
        }
    }


    [Fact]
    public async Task GetPlayerById_ShouldReturnNull_WhenPlayerDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.GetPlayerById(999);

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task TransferPlayer_ShouldUpdatePlayerClub()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var player = new Player { Name = "Player 1", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.TransferPlayer(1, 2);

            Assert.True(result);
            var updatedPlayer = await context.Players.FindAsync(1);
            Assert.Equal(2, updatedPlayer.ClubId);
        }
    }

    [Fact]
    public async Task TransferPlayer_ShouldReturnFalse_WhenPlayerDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var result = await service.TransferPlayer(999, 2);

            Assert.False(result);
        }
    }

    [Fact]
    public async Task UpdatePlayer_ShouldModifyExistingPlayer()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var player = new Player { Name = "Old Player", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var player = await context.Players.FirstAsync();
            player.Name = "Updated Player";
            player.Position = "Midfielder";
            var result = await service.UpdatePlayer(player);

            Assert.NotNull(result);
            Assert.Equal("Updated Player", result.Name);
            Assert.Equal("Midfielder", result.Position);
        }
    }

    [Fact]
    public async Task UpdatePlayer_ShouldReturnNull_WhenPlayerDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new PlayerService(context);
            var player = new Player { Id = 999, Name = "Non-Existent Player", Position = "Non-Existent Position", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };

            var result = await service.UpdatePlayer(player);

            Assert.Null(result);
        }
    }
}
