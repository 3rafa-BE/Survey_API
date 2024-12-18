using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Contracts.Result;
using Survey.Persestance;

namespace Survey.Services
{
    public class ResultService(DBContext dBContext) : IResultService
    {
        private readonly DBContext _dBContext = dBContext;

        public async Task<Result<PollVoteResponse>> GetVotesAsync(int pollid, CancellationToken cancellationToken)
        {
            var pollvotes = await _dBContext.polls
                .Where(x => x.Id == pollid)
                .Select(x => new PollVoteResponse
                (x.Title,
                x.Votes.Select(v => new VoteResponse($"{v.User.FirstName} {v.User.LastName}",
                v.SubmittedOn
                , v.VoteAnswers.
                Select(a => new QuestionAnswerRespone(a.Question.Content, a.Answer.Content))
                ))
                ))
                .SingleOrDefaultAsync(cancellationToken);
            return pollvotes is null ? Result.Failure<PollVoteResponse>(PollErrors.PollNotFound)
                : Result.Success(pollvotes);
        }
    }
}
