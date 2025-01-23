using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Abstractions;
using Survey.Services;

namespace Survey.Controllers
{
    [Route("api/poll/{pollid}/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultsController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;
        [HttpGet("raw-data")]
        public async Task<IActionResult> RawData([FromRoute] int pollid , CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVotesAsync(pollid, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(); 
        }
        [HttpGet("Per-Day")]
        public async Task<IActionResult> VotesPerDay([FromRoute] int pollid, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVotePerDayAsync(pollid, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("Per-Question")]
        public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollid, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVotePerQuestionAsync(pollid, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
