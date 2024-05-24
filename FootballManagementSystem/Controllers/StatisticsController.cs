using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsService _statisticsService;

    public StatisticsController(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("club-rankings")]
    public async Task<ActionResult<IEnumerable<ClubStatistics>>> GetClubRankings()
    {
        var rankings = await _statisticsService.GetClubRankings();
        return Ok(rankings);
    }

    [HttpGet("player-statistics")]
    public async Task<ActionResult<IEnumerable<PlayerStatistics>>> GetPlayerStatistics()
    {
        var statistics = await _statisticsService.GetPlayerStatistics();
        return Ok(statistics);
    }
}
