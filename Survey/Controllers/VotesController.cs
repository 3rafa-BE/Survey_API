using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Abstractions;
using Survey.Contracts.Vote;
using Survey.Services;

namespace Survey.Controllers
{
    [Route("api/poll/{pollid}/Vote")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionsServices questionsServices , IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionsServices _questionsServices = questionsServices;
        private readonly IVoteService _voteServices = voteService;

        [HttpGet("Get-Available-Votes")]
        public async Task<IActionResult> GetAvailabele([FromRoute] int pollid , CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _questionsServices.GetAvailableAsync(pollid, userId!, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("Add-Votes")]
        public async Task<IActionResult> AddAsync([FromRoute] int pollid ,[FromBody] VoteRequest request,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _voteServices.AddAsync(pollid , userId! , request, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();
        }
    }
}
