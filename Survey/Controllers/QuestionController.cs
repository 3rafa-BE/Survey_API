using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Abstractions;
using Survey.Contracts.Question;
using Survey.Services;

namespace Survey.Controllers
{
    [Route("api/polls/{pollid}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController(IQuestionsServices questionsServices) : ControllerBase
    {
        private readonly IQuestionsServices _questionsServices = questionsServices;
        
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromRoute] int pollid , CancellationToken cancellationToken)
        {
            var result = await _questionsServices.GetAllAsync(pollid , cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int pollid , [FromRoute] int id , CancellationToken cancellationToken)
        {
            var result = await _questionsServices.GetByIdAsync(pollid, id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value)
                : result.ToProblem();
        }
        [HttpPost("")]
        public async Task<IActionResult> Addasync([FromRoute] int pollid, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionsServices.AddAsync(pollid, request, cancellationToken);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(Get), new { pollid, result.Value.id }, result.Value);
            return result.ToProblem();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id , [FromRoute] int pollid , [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionsServices.UpdateAsync(pollid,id,request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPut("toggle/{id}")]
        public async Task<IActionResult> TogglePublisAsync([FromRoute]int pollid, [FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _questionsServices.Togglestatue(pollid , id , cancellationToken);
            return result.IsFailure ? NotFound(result.Error) : result.ToProblem();
        }
    }
}
