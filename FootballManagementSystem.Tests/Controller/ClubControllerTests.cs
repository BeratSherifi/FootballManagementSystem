using FootballManagementSystem.Controllers;
using FootballManagementSystem.Dto;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
        var clubDto = new CreateClubDto { Name = "Test Club", Stadium = "Test Stadium" };
        var club = new Club { Id = 1, Name = "Test Club", Stadium = "Test Stadium" };
        _mockService.Setup(service => service.CreateClub(It.IsAny<Club>())).ReturnsAsync(club);

        var result = await _controller.CreateClub(clubDto);

        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Club>(actionResult.Value);
        Assert.Equal("Test Club", returnValue.Name);
        Assert.Equal("Test Stadium", returnValue.Stadium);
    }

    [Fact]
    public async Task CreateClub_ShouldReturnConflict_WhenClubAlreadyExists()
    {
        var clubDto = new CreateClubDto { Name = "Duplicate Club", Stadium = "Stadium" };
        _mockService.Setup(service => service.CreateClub(It.IsAny<Club>())).ThrowsAsync(new DuplicateNameException("Duplicate club"));

        var result = await _controller.CreateClub(clubDto);

        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("Duplicate club", conflictResult.Value);
    }

    [Fact]
    public async Task CreateClub_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        var clubDto = new CreateClubDto { Name = "", Stadium = "Stadium" };
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.CreateClub(clubDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteClub_ShouldReturnNoContentResult_WhenClubIsDeleted()
    {
        _mockService.Setup(service => service.DeleteClub(It.IsAny<int>())).ReturnsAsync(true);

        var result = await _controller.DeleteClub(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteClub_ShouldReturnNotFoundResult_WhenClubDoesNotExist()
    {
        _mockService.Setup(service => service.DeleteClub(It.IsAny<int>())).ReturnsAsync(false);

        var result = await _controller.DeleteClub(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllClubs_ShouldReturnOkObjectResult_WithAllClubs()
    {
        var clubs = new List<Club>
        {
            new Club { Id = 1, Name = "Club 1", Stadium = "Stadium 1" },
            new Club { Id = 2, Name = "Club 2", Stadium = "Stadium 2" }
        };
        _mockService.Setup(service => service.GetAllClubs()).ReturnsAsync(clubs);

        var result = await _controller.GetAllClubs();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Club>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetClubById_ShouldReturnNotFoundResult_WhenClubDoesNotExist()
    {
        _mockService.Setup(service => service.GetClubById(It.IsAny<int>())).ReturnsAsync((Club)null);

        var result = await _controller.GetClubById(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetClubById_ShouldReturnOkObjectResult_WhenClubExists()
    {
        var club = new Club { Id = 1, Name = "Test Club", Stadium = "Test Stadium" };
        _mockService.Setup(service => service.GetClubById(It.IsAny<int>())).ReturnsAsync(club);

        var result = await _controller.GetClubById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Club>(okResult.Value);
        Assert.Equal("Test Club", returnValue.Name);
    }

    [Fact]
    public async Task GetPlayersOfClub_ShouldReturnNotFoundResult_WhenNoPlayersExist()
    {
        _mockService.Setup(service => service.GetPlayersOfClub(It.IsAny<int>())).ReturnsAsync(new List<Player>());

        var result = await _controller.GetPlayersOfClub(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPlayersOfClub_ShouldReturnOkObjectResult_WithPlayers()
    {
        var players = new List<Player>
        {
            new Player { Id = 1, Name = "Player 1", Position = "Forward" },
            new Player { Id = 2, Name = "Player 2", Position = "Midfielder" }
        };
        _mockService.Setup(service => service.GetPlayersOfClub(It.IsAny<int>())).ReturnsAsync(players);

        var result = await _controller.GetPlayersOfClub(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Player>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task UpdateClub_ShouldReturnNoContentResult_WhenClubIsUpdated()
    {
        var clubDto = new CreateClubDto { Name = "Updated Club", Stadium = "Updated Stadium" };
        var club = new Club { Id = 1, Name = "Updated Club", Stadium = "Updated Stadium" };
        _mockService.Setup(service => service.UpdateClub(It.IsAny<Club>())).ReturnsAsync(club);

        var result = await _controller.UpdateClub(1, clubDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateClub_ShouldReturnNotFoundResult_WhenClubDoesNotExist()
    {
        var clubDto = new CreateClubDto { Name = "Updated Club", Stadium = "Updated Stadium" };
        _mockService.Setup(service => service.UpdateClub(It.IsAny<Club>())).ReturnsAsync((Club)null);

        var result = await _controller.UpdateClub(1, clubDto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateClub_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        var clubDto = new CreateClubDto { Name = "", Stadium = "Updated Stadium" };
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.UpdateClub(1, clubDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
