using FootballManagementSystem.Models;

namespace FootballManagementSystem.Services;

public interface IClubService
{
    Task<IEnumerable<Club>> GetClubs();
    Task<Club> GetClubById(int id);
    Task<IEnumerable<Club>> GetAllClubs();
    Task<Club> CreateClub(Club club);
    Task<Club> UpdateClub(Club club);
    Task<bool> DeleteClub(int id);
    Task<IEnumerable<Player>> GetPlayersOfClub(int clubId);
}