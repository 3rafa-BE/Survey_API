using Mapster;
using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Contracts.Question;
using Survey.Contracts.Vote;
using Survey.Models;
using Survey.Persestance;

namespace Survey.Services
{
    public class VoteService(DBContext dBContext) : IVoteService
    {
        private readonly DBContext _dBContext = dBContext;

        public async Task<Result> AddAsync(int pollid, string userid, VoteRequest voteRequest, CancellationToken cancellationToken)
        {
            var HasVoted = await _dBContext.votes.AnyAsync(x => x.pollid == pollid && x.UserId == userid, cancellationToken);
            if (HasVoted)
                return Result.Failure(VoteError.DuplicatedVoting);
            var pollExists = await _dBContext.polls.AnyAsync(x => x.Id == pollid
            && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
            && DateOnly.FromDateTime(DateTime.UtcNow) <= x.EndsAt);
            if (!pollExists)
                return Result.Failure(PollErrors.PollNotFound);
            var AvailableQuestions = await _dBContext
                .questions.Where(x=>x.Pollid == pollid && x.IsActive)
                .Select(x=>x.Id)
                .ToListAsync(cancellationToken);
            if(!voteRequest.Answers.Select(x=>x.QuestionId).SequenceEqual(AvailableQuestions))
                return Result.Failure(VoteError.InvalidQuestion);
            var vote = new Vote
            {
                pollid = pollid,
                UserId = userid,
                VoteAnswers = voteRequest.Answers.Adapt<IEnumerable<VoteAnswers>>().ToList()
            };
            await _dBContext.votes.AddAsync(vote , cancellationToken);
            await _dBContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
