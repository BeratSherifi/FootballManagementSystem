using FootballManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballManagementSystem.Services;

public class PlayerService : IPlayerService
{
    private readonly FootballContext _context;

    public PlayerService(FootballContext context)
    {
        _context = context;
    }

    public async Task<Player> CreatePlayer(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Player> UpdatePlayer(Player player)
    {
        var existingPlayer = await _context.Players.FindAsync(player.Id);
        if (existingPlayer == null)
        {
            return null;
        }

        existingPlayer.Name = player.Name;
        existingPlayer.Position = player.Position;
        existingPlayer.ClubId = player.ClubId;
        existingPlayer.Goals = player.Goals;
        existingPlayer.Assists = player.Assists;
        existingPlayer.Appearances = player.Appearances;

        await _context.SaveChangesAsync();
        return existingPlayer;
    }

    public async Task<Player> GetPlayerById(int id)
    {
        return await _context.Players
            .Include(p => p.Club)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Player>> GetAllPlayers()
    {
        return await _context.Players
            .Include(p => p.Club)
            .ToListAsync();
    }

    public async Task<bool> DeletePlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return false;
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TransferPlayer(int playerId, int newClubId)
    {
        var player = await _context.Players.FindAsync(playerId);
        if (player == null)
        {
            return false;
        }

        player.ClubId = newClubId;
        await _context.SaveChangesAsync();
        return true;
    }
}
