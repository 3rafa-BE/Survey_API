using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Abstractions;
using Survey.Contracts.Poll;
using Survey.Models;
using Survey.Services;

namespace Survey.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollController : ControllerBase
    {
        private IPollServices _PollServices;
        public PollController(IPollServices pollServices)
        {
            _PollServices = pollServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken) => 
            Ok(await _PollServices.GetAllAsync(cancellationToken));
        [HttpGet("Current")]
        public async Task<IActionResult> GetCurrentAsync(CancellationToken cancellationToken) =>
            Ok(await _PollServices.GetAllCurrentAsync(cancellationToken));
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _PollServices.GetAsync(id , cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] PollRequest request , CancellationToken cancellationToken)
        {
            var result = await _PollServices.AddAsync(request, cancellationToken);

            return result.IsSuccess 
                ? CreatedAtAction(nameof(GetAsync), new { id = result.Value.id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PollRequest request , CancellationToken cancellationToken)
        {
            var result = await _PollServices.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id , CancellationToken cancellationToken)
        {
            var result = await _PollServices.DeleteAsync(id , cancellationToken);
            return result.IsFailure ? NotFound(result.Error) : result.ToProblem();

        }
        [HttpPut("toggle/{id}")]
        public async Task<IActionResult> TogglePublisAsync(int id , CancellationToken cancellationToken)
        {
            var result = await _PollServices.TogglePublishedAsync(id, cancellationToken);
            return result.IsFailure ? NotFound(result.Error) : result.ToProblem();
        }
    }
}
