using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FootballManagementSystem;
using Xunit;

public class ClubServiceTests
{
    private DbContextOptions<FootballContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<FootballContext>()
            .UseInMemoryDatabase(databaseName: $"FootballTestDb_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task GetClubs_ShouldReturnAllClubs()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            context.Clubs.AddRange(new Club { Name = "Club 1", Stadium = "Stadium 1" }, new Club { Name = "Club 2", Stadium = "Stadium 2" });
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.GetClubs();

            Assert.Equal(2, result.Count());
        }
    }

    [Fact]
    public async Task GetClubById_ShouldReturnClub_WhenClubExists()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var club = new Club { Name = "Club 1", Stadium = "Stadium 1" };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.GetClubById(1);

            Assert.NotNull(result);
            Assert.Equal("Club 1", result.Name);
        }
    }

    [Fact]
    public async Task GetClubById_ShouldReturnNull_WhenClubDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.GetClubById(999);

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task CreateClub_ShouldAddNewClub()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var club = new Club { Name = "New Club", Stadium = "New Stadium" };

            var result = await service.CreateClub(club);

            Assert.NotNull(result);
            Assert.Equal("New Club", result.Name);
            Assert.Equal("New Stadium", result.Stadium);
        }

        using (var context = new FootballContext(options))
        {
            Assert.Equal(1, await context.Clubs.CountAsync());
        }
    }

    [Fact]
    public async Task CreateClub_ShouldThrowException_WhenAddingDuplicateClub()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var club = new Club { Name = "Duplicate Club", Stadium = "Stadium" };

            await service.CreateClub(club);

            await Assert.ThrowsAsync<DuplicateNameException>(async () =>
            {
                await service.CreateClub(club);
            });
        }
    }

    [Fact]
    public async Task UpdateClub_ShouldModifyExistingClub()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var club = new Club { Name = "Old Club", Stadium = "Old Stadium" };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var club = await context.Clubs.FirstAsync();
            club.Name = "Updated Club";
            club.Stadium = "Updated Stadium";
            var result = await service.UpdateClub(club);

            Assert.NotNull(result);
            Assert.Equal("Updated Club", result.Name);
            Assert.Equal("Updated Stadium", result.Stadium);
        }
    }

    [Fact]
    public async Task UpdateClub_ShouldReturnNull_WhenClubDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var club = new Club { Id = 999, Name = "Non-Existent Club", Stadium = "Non-Existent Stadium" };

            var result = await service.UpdateClub(club);

            Assert.Null(result);
        }
    }

    [Fact]
    public async Task DeleteClub_ShouldRemoveClub()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var club = new Club { Name = "Club to Delete", Stadium = "Stadium" };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.DeleteClub(1);

            Assert.True(result);
            Assert.Null(await context.Clubs.FindAsync(1));
        }
    }

    [Fact]
    public async Task DeleteClub_ShouldReturnFalse_WhenClubDoesNotExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.DeleteClub(999);

            Assert.False(result);
        }
    }

    [Fact]
    public async Task GetPlayersOfClub_ShouldReturnPlayers_WhenPlayersExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var club = new Club { Name = "Club with Players", Stadium = "Stadium" };
            context.Clubs.Add(club);
            context.Players.AddRange(new Player { Name = "Player 1", Position = "Forward", Club = club }, new Player { Name = "Player 2", Position = "Midfielder", Club = club });
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.GetPlayersOfClub(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }

    [Fact]
    public async Task GetPlayersOfClub_ShouldReturnEmpty_WhenNoPlayersExist()
    {
        var options = CreateNewContextOptions();

        using (var context = new FootballContext(options))
        {
            var club = new Club { Name = "Empty Club", Stadium = "Empty Stadium" };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();
        }

        using (var context = new FootballContext(options))
        {
            var service = new ClubService(context);
            var result = await service.GetPlayersOfClub(1);

            Assert.Empty(result);
        }
    }
}
