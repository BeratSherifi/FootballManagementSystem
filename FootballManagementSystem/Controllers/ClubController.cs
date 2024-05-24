using FootballManagementSystem.Dto;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;

namespace FootballManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpPost]
        public async Task<ActionResult<Club>> CreateClub(CreateClubDto clubDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var club = new Club
            {
                Name = clubDto.Name,
                Stadium = clubDto.Stadium
            };

            try
            {
                var createdClub = await _clubService.CreateClub(club);
                return CreatedAtAction(nameof(GetClubById), new { id = createdClub.Id }, createdClub);
            }
            catch (DuplicateNameException)
            {
                return Conflict("Duplicate club");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClub(int id, CreateClubDto clubDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var club = new Club
            {
                Id = id,
                Name = clubDto.Name,
                Stadium = clubDto.Stadium
            };

            var result = await _clubService.UpdateClub(club);

            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> GetClubById(int id)
        {
            var club = await _clubService.GetClubById(id);
            if (club == null)
            {
                return NotFound();
            }

            return Ok(club);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetAllClubs()
        {
            var clubs = await _clubService.GetAllClubs();
            return Ok(clubs);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var result = await _clubService.DeleteClub(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/players")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersOfClub(int id)
        {
            var players = await _clubService.GetPlayersOfClub(id);
            if (players == null || !players.Any())
            {
                return NotFound();
            }
            return Ok(players);
        }
    }
}
