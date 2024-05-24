using System.Collections.Generic;
using System.Threading.Tasks;
using FootballManagementSystem.Controllers;
using FootballManagementSystem.Dto;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ClubControllerTests
{
    private readonly Mock<IClubService> _mockService;
    private readonly ClubController _controller;

    public ClubControllerTests()
    {
        _mockService = new Mock<IClubService>();
        _controller = new ClubController(_mockService.Object);
    }

    [Fact]
    public async Task CreateClub_ShouldReturnCreatedAtActionResult_WhenClubIsCreated()
    {
        // Arrange
        var clubDto = new CreateClubDto { Name = "Test Club", Stadium = "Test Stadium" };
        var club = new Club { Id = 1, Name = "Test Club", Stadium = "Test Stadium" };
        _mockService.Setup(service => service.CreateClub(It.IsAny<Club>())).ReturnsAsync(club);

        // Act
        var result = await _controller.CreateClub(clubDto);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Club>(actionResult.Value);
        Assert.Equal("Test Club", returnValue.Name);
        Assert.Equal("Test Stadium", returnValue.Stadium);
    }

    [Fact]
    public async Task DeleteClub_ShouldReturnNoContentResult_WhenClubIsDeleted()
    {
        // Arrange
        _mockService.Setup(service => service.DeleteClub(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteClub(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteClub_ShouldReturnNotFoundResult_WhenClubDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.DeleteClub(It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteClub(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllClubs_ShouldReturnOkObjectResult_WithAllClubs()
    {
        // Arrange
        var clubs = new List<Club>
        {
            new Club { Id = 1, Name = "Club 1", Stadium = "Stadium 1" },
            new Club { Id = 2, Name = "Club 2", Stadium = "Stadium 2" }
        };
        _mockService.Setup(service => service.GetAllClubs()).ReturnsAsync(clubs);

        // Act
        var result = await _controller.GetAllClubs();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Club>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetClubById_ShouldReturnNotFoundResult_WhenClubDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.GetClubById(It.IsAny<int>())).ReturnsAsync((Club)null);

        // Act
        var result = await _controller.GetClubById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetClubById_ShouldReturnOkObjectResult_WhenClubExists()
    {
        // Arrange
        var club = new Club { Id = 1, Name = "Test Club", Stadium = "Test Stadium" };
        _mockService.Setup(service => service.GetClubById(It.IsAny<int>())).ReturnsAsync(club);

        // Act
        var result = await _controller.GetClubById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Club>(okResult.Value);
        Assert.Equal("Test Club", returnValue.Name);
    }


    [Fact]
    public async Task GetPlayersOfClub_ShouldReturnNotFoundResult_WhenNoPlayersExist()
    {
        // Arrange
        _mockService.Setup(service => service.GetPlayersOfClub(It.IsAny<int>())).ReturnsAsync(new List<Player>());

        // Act
        var result = await _controller.GetPlayersOfClub(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPlayersOfClub_ShouldReturnOkObjectResult_WithPlayers()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Id = 1, Name = "Player 1", Position = "Forward" },
            new Player { Id = 2, Name = "Player 2", Position = "Midfielder" }
        };
        _mockService.Setup(service => service.GetPlayersOfClub(It.IsAny<int>())).ReturnsAsync(players);

        // Act
        var result = await _controller.GetPlayersOfClub(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Player>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task UpdateClub_ShouldReturnNoContentResult_WhenClubIsUpdated()
    {
        // Arrange
        var clubDto = new UpdateClubDto { Name = "Updated Club", Stadium = "Updated Stadium" };
        var club = new Club { Id = 1, Name = "Updated Club", Stadium = "Updated Stadium" };
        _mockService.Setup(service => service.UpdateClub(It.IsAny<Club>())).ReturnsAsync(club);

        // Act
        var result = await _controller.UpdateClub(1, clubDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateClub_ShouldReturnNotFoundResult_WhenClubDoesNotExist()
    {
        // Arrange
        var clubDto = new UpdateClubDto { Name = "Updated Club", Stadium = "Updated Stadium" };
        _mockService.Setup(service => service.UpdateClub(It.IsAny<Club>())).ReturnsAsync((Club)null);

        // Act
        var result = await _controller.UpdateClub(1, clubDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
