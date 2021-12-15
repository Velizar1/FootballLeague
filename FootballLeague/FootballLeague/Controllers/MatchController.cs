﻿using FootballLeague.Core.Contracts;
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

        [HttpGet()]
        public ActionResult<List<MatchModel<Guid>>> GetAll()
        {
            try
            {
                var result = matchService.AllMatches<Guid>();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<MatchModel<Guid>>> GetTeamById(Guid Id)
        {
            try
            {
                var result = await matchService.GetMatchByIdAsync(Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post(MatchModel<Guid> match)
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
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(Guid Id, MatchModel<Guid> match)
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
                var result = await matchService.RemoveMatchAsync(Id);
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