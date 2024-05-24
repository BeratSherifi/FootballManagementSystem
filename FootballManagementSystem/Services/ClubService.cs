using FootballManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FootballManagementSystem.Services
{
    public class ClubService : IClubService
    {
        private readonly FootballContext _context;

        public ClubService(FootballContext context)
        {
            _context = context;
        }

        public async Task<bool> ClubExists(string name)
        {
            return await _context.Clubs.AnyAsync(c => c.Name == name);
        }

        public async Task<Club> CreateClub(Club club)
        {
            if (await ClubExists(club.Name))
            {
                throw new DuplicateNameException("Duplicate club");
            }

            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task<IEnumerable<Club>> GetClubs()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetClubById(int id)
        {
            return await _context.Clubs
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetAllClubs()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> UpdateClub(Club club)
        {
            var existingClub = await _context.Clubs.FindAsync(club.Id);
            if (existingClub == null)
            {
                return null;
            }

            existingClub.Name = club.Name;
            existingClub.Stadium = club.Stadium;

            await _context.SaveChangesAsync();
            return existingClub;
        }

        public async Task<bool> DeleteClub(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return false;
            }

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Player>> GetPlayersOfClub(int clubId)
        {
            return await _context.Players
                .Where(p => p.ClubId == clubId)
                .ToListAsync();
        }
    }
}
