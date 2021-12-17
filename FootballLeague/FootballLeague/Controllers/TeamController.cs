using FootballLeague.Core.Commons;
using FootballLeague.Core.Constants;
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
        /// <summary>
        /// Return list of all team records
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult GetAll()
        {
            try
            {
                var result = teamService.AllTeams();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Get a team by given Id
        /// </summary>
        /// <param name="Id">Identificator of the team</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async  Task<ActionResult> GetTeamById(Guid Id)
        {
            try
            {
                var result = await teamService.GetTeamByIdAsync(Id);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Add a new record 
        /// </summary>
        /// <param name="team">model of the entity</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(TeamAddModel team)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
                 result = await teamService.AddTeamAsync(team);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Update an existing record
        /// </summary>
        /// <param name="Id">Identificator of a team</param>
        /// <param name="team">model of the entity</param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(Guid Id, TeamEditModel team)
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
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Removes a record
        /// </summary>
        /// <param name="Id">Identificator of a team</param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
