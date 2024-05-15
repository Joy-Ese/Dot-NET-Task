using DotNetTask.Models.DTOs;
using DotNetTask.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private IEmployerService _employerService;

        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpPost("AddNewCustomQuestion")]
        public async Task<IActionResult> AddCustomQuestion([FromBody] NewQuestion req)
        {
            var result = await _employerService.AddCustomQuestion(req);
            return Ok(result);
        }

    }
}
