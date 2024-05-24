using FootballManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballManagementSystem.Services
{
    public interface IClubService
    {
        Task<bool> ClubExists(string name);
        Task<Club> CreateClub(Club club);
        Task<IEnumerable<Club>> GetClubs();
        Task<Club> GetClubById(int id);
        Task<IEnumerable<Club>> GetAllClubs();
        Task<Club> UpdateClub(Club club);
        Task<bool> DeleteClub(int id);
        Task<IEnumerable<Player>> GetPlayersOfClub(int clubId);
    }
}