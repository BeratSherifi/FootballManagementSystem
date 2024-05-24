using FootballManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballManagementSystem.Services;

public class StatisticsService
{
    private readonly FootballContext _context;

    public StatisticsService(FootballContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClubStatistics>> GetClubRankings()
    {
        var clubRankings = await _context.Clubs
            .Select(c => new ClubStatistics
            {
                Club = c,
                Points = _context.Matches
                    .Where(m => m.HomeClubId == c.Id || m.AwayClubId == c.Id)
                    .Select(m => new
                    {
                        ClubId = m.HomeClubId == c.Id ? m.HomeClubId : m.AwayClubId,
                        Points = (m.HomeClubId == c.Id && m.HomeClubGoals > m.AwayClubGoals) ||
                                 (m.AwayClubId == c.Id && m.AwayClubGoals > m.HomeClubGoals) ? 3 :
                            (m.HomeClubGoals == m.AwayClubGoals ? 1 : 0)
                    }).Sum(m => m.Points)
            })
            .OrderByDescending(c => c.Points)
            .ToListAsync();

        return clubRankings;
    }

    
    public async Task<IEnumerable<PlayerStatistics>> GetPlayerStatistics()
    {
        return await _context.Players
            .Select(p => new PlayerStatistics
            {
                Player = p,
                Goals = p.Goals,
                Assists = p.Assists,
                Appearances = p.Appearances
            })
            .OrderByDescending(p => p.Goals)
            .ThenByDescending(p => p.Assists)
            .ToListAsync();
    }
}
