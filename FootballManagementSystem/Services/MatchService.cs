using FootballManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballManagementSystem.Services;

public class MatchService
{
    private readonly FootballContext _context;

    public MatchService(FootballContext context)
    {
        _context = context;
    }

    public async Task<Match> ScheduleMatch(Match match)
    {
        _context.Matches.Add(match);
        await _context.SaveChangesAsync();
        return match;
    }

    public async Task<Match> GetMatchById(int id)
    {
        return await _context.Matches
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Events)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Match>> GetAllMatches()
    {
        return await _context.Matches
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Events)
            .ToListAsync();
    }

    public async Task<bool> RecordMatchResult(int matchId, List<Event> events)
    {
        var match = await _context.Matches.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == matchId);
        if (match == null)
        {
            return false;
        }

        foreach (var e in events)
        {
            e.MatchId = matchId;
        }

        _context.Events.AddRange(events);
        await _context.SaveChangesAsync();
        return true;
    }


}
