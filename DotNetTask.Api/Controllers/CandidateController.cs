﻿using DotNetTask.Models.DTOs;
using DotNetTask.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("AddNewCustomQuestion")]
        public async Task<IActionResult> AddCustomQuestion([FromBody] NewQuestion req)
        {
            var result = await _candidateService.AddCustomQuestion(req);
            return Ok(result);
        }

        [HttpPost("AddNewProgram")]
        public async Task<IActionResult> AddNewProgram([FromBody] NewProgram req)
        {
            var result = await _candidateService.AddNewProgram(req);
            return Ok(result);
        }

        [HttpGet("GetAllCandidates")]
        public async Task<IActionResult> GetAllCandidates()
        {
            var result = await _candidateService.GetAllCandidates();
            return Ok(result);
        }

        [HttpGet("GetQuestionByType")]
        public async Task<IActionResult> GetQuestionByType(string type)
        {
            var result = await _candidateService.GetQuestionByType(type);
            return Ok(result);
        }

        [HttpPut("UpdateCandidateQuestion")]
        public async Task<IActionResult> UpdateCandidateQuestion(string id, string quest)
        {
            var result = await _candidateService.UpdateCandidateQuestion(id, quest);
            return Ok(result);
        }
    }
}
