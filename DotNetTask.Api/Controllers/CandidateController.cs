using DotNetTask.Models.DTOs;
using DotNetTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost("AddNewCandidate")]
        public async Task<IActionResult> AddCandidate([FromBody] NewCandidate req)
        {
            var result = await _candidateService.AddCandidate(req);
            return Ok(result);
        }

        [HttpGet("GetAllCandidates")]
        public async Task<IActionResult> GetAllCandidates()
        {
            var result = await _candidateService.GetAllCandidates();
            return Ok(result);
        }

    }
}
