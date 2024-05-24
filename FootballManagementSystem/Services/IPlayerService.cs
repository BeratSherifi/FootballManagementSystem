using FootballManagementSystem.Models;

namespace FootballManagementSystem.Services;

public interface IPlayerService
{
    Task<Player> CreatePlayer(Player player);
    Task<Player> UpdatePlayer(Player player);
    Task<Player> GetPlayerById(int id);
    Task<IEnumerable<Player>> GetAllPlayers();
    Task<bool> DeletePlayer(int id);
    Task<bool> TransferPlayer(int playerId, int newClubId);
}