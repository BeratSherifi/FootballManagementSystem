using FootballManagementSystem.Dto;
using FootballManagementSystem.Models;
using FootballManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(CreatePlayerDto playerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (playerDto == null)
            {
                return BadRequest("Player data is null");
            }

            var player = new Player
            {
                Name = playerDto.Name,
                Position = playerDto.Position,
                ClubId = playerDto.ClubId,
                Goals = playerDto.Goals,
                Assists = playerDto.Assists,
                Appearances = playerDto.Appearances
            };

            var createdPlayer = await _playerService.CreatePlayer(player);
            return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayer.Id }, createdPlayer);
        }


        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerDto playerDto)
        {
            var player = new Player
            {
                Id = id,
                Name = playerDto.Name,
                Position = playerDto.Position,
                ClubId = playerDto.ClubId,
                Goals = playerDto.Goals,
                Assists = playerDto.Assists,
                Appearances = playerDto.Appearances
            };

            var result = await _playerService.UpdatePlayer(player);

            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        */
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerDto playerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = new Player
            {
                Id = id,
                Name = playerDto.Name,
                Position = playerDto.Position,
                ClubId = playerDto.ClubId,
                Goals = playerDto.Goals,
                Assists = playerDto.Assists,
                Appearances = playerDto.Appearances
            };

            var result = await _playerService.UpdatePlayer(player);
            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerById(int id)
        {
            var player = await _playerService.GetPlayerById(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
        {
            var players = await _playerService.GetAllPlayers();
            return Ok(players);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var result = await _playerService.DeletePlayer(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> TransferPlayer(int id, [FromBody] int newClubId)
        {
            var result = await _playerService.TransferPlayer(id, newClubId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
