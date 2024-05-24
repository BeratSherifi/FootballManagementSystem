using FootballManagementSystem.Controllers;
using FootballManagementSystem.Dto;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class PlayerControllerTests
{
    private readonly Mock<IPlayerService> _mockService;
    private readonly PlayerController _controller;

    public PlayerControllerTests()
    {
        _mockService = new Mock<IPlayerService>();
        _controller = new PlayerController(_mockService.Object);
    }

    [Fact]
    public async Task CreatePlayer_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.CreatePlayer(new CreatePlayerDto());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }


    [Fact]
    public async Task CreatePlayer_ShouldReturnCreatedAtActionResult_WhenPlayerIsCreated()
    {
        // Arrange
        var playerDto = new CreatePlayerDto { Name = "Test Player", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
        var player = new Player { Id = 1, Name = "Test Player", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
        _mockService.Setup(service => service.CreatePlayer(It.IsAny<Player>())).ReturnsAsync(player);

        // Act
        var result = await _controller.CreatePlayer(playerDto);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Player>(actionResult.Value);
        Assert.Equal("Test Player", returnValue.Name);
    }

    [Fact]
    public async Task DeletePlayer_ShouldReturnNoContentResult_WhenPlayerIsDeleted()
    {
        // Arrange
        _mockService.Setup(service => service.DeletePlayer(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _controller.DeletePlayer(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePlayer_ShouldReturnNotFoundResult_WhenPlayerDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.DeletePlayer(It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var result = await _controller.DeletePlayer(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllPlayers_ShouldReturnOkObjectResult_WithAllPlayers()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Id = 1, Name = "Player 1", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 },
            new Player { Id = 2, Name = "Player 2", Position = "Midfielder", ClubId = 1, Goals = 5, Assists = 7, Appearances = 15 }
        };
        _mockService.Setup(service => service.GetAllPlayers()).ReturnsAsync(players);

        // Act
        var result = await _controller.GetAllPlayers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Player>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetPlayerById_ShouldReturnNotFoundResult_WhenPlayerDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.GetPlayerById(It.IsAny<int>())).ReturnsAsync((Player)null);

        // Act
        var result = await _controller.GetPlayerById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPlayerById_ShouldReturnOkObjectResult_WhenPlayerExists()
    {
        // Arrange
        var player = new Player { Id = 1, Name = "Test Player", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
        _mockService.Setup(service => service.GetPlayerById(It.IsAny<int>())).ReturnsAsync(player);

        // Act
        var result = await _controller.GetPlayerById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Player>(okResult.Value);
        Assert.Equal("Test Player", returnValue.Name);
    }

    [Fact]
    public async Task TransferPlayer_ShouldReturnNoContentResult_WhenPlayerIsTransferred()
    {
        // Arrange
        _mockService.Setup(service => service.TransferPlayer(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _controller.TransferPlayer(1, 2);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task TransferPlayer_ShouldReturnNotFoundResult_WhenPlayerDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.TransferPlayer(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var result = await _controller.TransferPlayer(1, 2);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdatePlayer_ShouldReturnNoContentResult_WhenPlayerIsUpdated()
    {
        // Arrange
        var playerDto = new UpdatePlayerDto { Name = "Updated Player", Position = "Midfielder", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
        var player = new Player { Id = 1, Name = "Updated Player", Position = "Midfielder", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
        _mockService.Setup(service => service.UpdatePlayer(It.IsAny<Player>())).ReturnsAsync(player);

        // Act
        var result = await _controller.UpdatePlayer(1, playerDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePlayer_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var playerDto = new UpdatePlayerDto { Name = "", Position = "Midfielder", ClubId = 1, Goals = 5, Assists = 7, Appearances = 15 };
        var player = new Player { Id = 1, Name = "Player 1", Position = "Forward", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };

        // Ensure the club exists
        var club = new Club { Id = 1, Name = "Club 1", Stadium = "Stadium 1" };
        _mockService.Setup(service => service.GetPlayerById(1)).ReturnsAsync(player);
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.UpdatePlayer(1, playerDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }


    [Fact]
    public async Task UpdatePlayer_ShouldReturnNotFoundResult_WhenPlayerDoesNotExist()
    {
        // Arrange
        var playerDto = new UpdatePlayerDto { Name = "Updated Player", Position = "Midfielder", ClubId = 1, Goals = 10, Assists = 5, Appearances = 20 };
        _mockService.Setup(service => service.UpdatePlayer(It.IsAny<Player>())).ReturnsAsync((Player)null);

        // Act
        var result = await _controller.UpdatePlayer(1, playerDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
