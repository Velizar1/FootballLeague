using FootballLeague.Core.Contracts;
using FootballLeague.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService teamService;
        private readonly IMatchService matchService;

        public TeamController(ITeamService _teamService, IMatchService _matchService)
        {
            this.teamService = _teamService;
            this.matchService = _matchService;
        }

        [HttpGet()]
        public ActionResult<List<TeamModel<Guid>>> GetAll()
        {
            try
            {
                var result = teamService.AllTeams<Guid>();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpGet("{Id}")]
        public async  Task<ActionResult<TeamModel<Guid>>> GetTeamById(Guid Id)
        {
            try
            {
                var result = await teamService.GetTeamByIdAsync(Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post(TeamModel<Guid> team)
        {
            try
            {
                var result = await teamService.AddTeamAsync(team);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(Guid Id, TeamModel<Guid> team)
        {
            try
            {
                if (Id != team.Id)
                {
                    return BadRequest();
                }
                var result = await teamService.UdpateTeamAsync(team);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var result = await teamService.RemoveTeamAsync(Id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
        }
    }
}
