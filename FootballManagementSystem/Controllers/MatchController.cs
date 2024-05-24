using FootballManagementSystem.Dto;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly MatchService _matchService;

    public MatchController(MatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpPost]
    public async Task<ActionResult<Match>> ScheduleMatch(MatchDto matchDto)
    {
        var match = new Match
        {
            HomeClubId = matchDto.HomeClubId,
            AwayClubId = matchDto.AwayClubId,
            Date = matchDto.Date
        };

        var scheduledMatch = await _matchService.ScheduleMatch(match);
        return CreatedAtAction(nameof(GetMatchById), new { id = scheduledMatch.Id }, scheduledMatch);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Match>> GetMatchById(int id)
    {
        var match = await _matchService.GetMatchById(id);
        if (match == null)
        {
            return NotFound();
        }
        return match;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Match>>> GetAllMatches()
    {
        var matches = await _matchService.GetAllMatches();
        return Ok(matches);
    }

    [HttpPost("{id}/results")]
    public async Task<IActionResult> RecordMatchResult(int id, [FromBody] List<EventDto> eventDtos)
    {
        var events = eventDtos.Select(e => new Event
        {
            MatchId = id,
            PlayerId = e.PlayerId,
            Type = e.Type,
            Timestamp = e.Timestamp
        }).ToList();

        var result = await _matchService.RecordMatchResult(id, events);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }


}
