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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {

        private readonly IMatchService matchService;

        public MatchController(IMatchService _matchService)
        {
            matchService = _matchService;
        }

        /// <summary>
        /// Return list of all match records
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult GetAll()
        {

            try
            {
                var result = matchService.AllMatches();
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get a match by an Id
        /// </summary>
        /// <param name="Id">Identificator of the match</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMatchById(Guid id)
        {
            try
            {
                var result = await matchService.GetMatchByIdAsync(id);
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
        /// <param name="match">Model of a match</param>
        /// <returns>RepositoryResult</returns>
        [HttpPost]
        public async Task<ActionResult> Post(MatchModel match)
        {

            try
            {
                var result = await matchService.AddMatchAsync(match);
                if (result.IsSuccess)
                {
                    return Ok( result);
                }
                return BadRequest(result.Message);
            }
           
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        /// <summary>
        /// Update an existing record
        /// </summary>
        /// <param name="Id">Identificator of a match</param>
        /// <param name="match">Model of a match</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, MatchModel match)
        {
            try
            {
                if (!id.Equals(match.Id))
                {
                    return BadRequest("Ids are diffrent");
                }
                var result = await matchService.UdpateMatchAsync(match);
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
        /// <param name="Id">Identificator of a match</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await matchService.RemoveMatchAsync(id);
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
