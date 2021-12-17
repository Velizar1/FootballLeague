using FootballLeague.Core.Commons;
using FootballLeague.Core.Constants;
using FootballLeague.Core.Contracts;
using FootballLeague.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
           teamService = _teamService;
           matchService = _matchService;
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
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get a team by given Id
        /// </summary>
        /// <param name="Id">Identificator of the team</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async  Task<ActionResult> GetTeamById(Guid id)
        {
            try
            {
                var result = await teamService.GetTeamByIdAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Add a new record 
        /// </summary>
        /// <param name="team">Model of the entity</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(TeamModel team)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
                 result = await teamService.AddTeamAsync(team);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update an existing record
        /// </summary>
        /// <param name="Id">Identificator of a team</param>
        /// <param name="team">Model of a team</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, TeamModel team)
        {
            try
            {
                if (!id.Equals(team.Id))
                {
                    return BadRequest("Ids are diffrent");
                }
                var result = await teamService.UdpateTeamAsync(team);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Removes a record
        /// </summary>
        /// <param name="Id">Identificator of a team</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await teamService.RemoveTeamAsync(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result.Message);
            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
