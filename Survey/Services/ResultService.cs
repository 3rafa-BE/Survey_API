using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Contracts.Result;
using Survey.Persestance;
using System.Collections.Generic;

namespace Survey.Services
{
    public class ResultService(DBContext dBContext) : IResultService
    {
        private readonly DBContext _dBContext = dBContext;

        public async Task<Result<IEnumerable<VotePerDayResponse>>> GetVotePerDayAsybc(int pollid, CancellationToken cancellationToken)
        {
            var pollisExisted = await _dBContext.polls.AnyAsync(x=>x.Id == pollid , cancellationToken);
            if(!pollisExisted)
                return Result.Failure<IEnumerable<VotePerDayResponse>>(PollErrors.PollNotFound);
            var VotesPerDay = await _dBContext.votes
                .Where(x => x.pollid == pollid)
                .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
                .Select(g => new VotePerDayResponse(
                    g.Key.Date,
                    g.Count()
                    )).ToListAsync(cancellationToken);
            return Result.Success<IEnumerable<VotePerDayResponse>>(VotesPerDay);
        }

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
