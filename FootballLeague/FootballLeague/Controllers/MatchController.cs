using FootballLeague.Core.Constants;
using FootballLeague.Core.Contracts;
using FootballLeague.Core.Models;
using Microsoft.AspNetCore.Mvc;
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
            
            this.matchService = _matchService;
        }
        /// <summary>
        /// Return list of all match records
        /// </summary>
        /// <returns>List<MatchModelEdit<T>></returns>
        [HttpGet()]
        public ActionResult GetAll()
        {
            try
            {
                var result = matchService.AllMatches();

                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound(ResultConstants.NotFound);
            }
        }
        /// <summary>
        /// Get a match by given Id
        /// </summary>
        /// <param name="Id">Identificator of the match</param>
        /// <returns>MatchEditModel<Guid></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetMatchById(Guid Id)
        {
            try
            {
                var result = await matchService.GetMatchByIdAsync(Id);

                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound(ResultConstants.NotFound);
            }
        }
        /// <summary>
        /// Add a new record 
        /// </summary>
        /// <param name="match">model of the entity</param>
        /// <returns>RepositoryResult</returns>
        [HttpPost]
        public async Task<ActionResult> Post(MatchAddModel match)
        {
            
            try
            {
                var result = await matchService.AddMatchAsync(match);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (Exception)
            {
                return NotFound(ResultConstants.NotFound);
            }
        }
        /// <summary>
        /// Update an existing record
        /// </summary>
        /// <param name="Id">identificator of a match</param>
        /// <param name="match">model of the entity</param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(Guid Id, MatchEditModel match)
        {
            try
            {

                if (Id != match.Id)
                {
                    return BadRequest();
                }
                var result = await matchService.UdpateMatchAsync(match);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (Exception)
            {
                return NotFound(ResultConstants.NotFound);
            }
        }
        /// <summary>
        /// Removes a record
        /// </summary>
        /// <param name="Id">Identificator of a match</param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var result = await matchService.RemoveMatchAsync(Id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return NotFound(result.Message);
            }
            catch (Exception)
            {
                return NotFound(ResultConstants.NotFound);
            }
        }
    }
}
