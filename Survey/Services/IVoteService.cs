using Survey.Abstractions;
using Survey.Contracts.Vote;

namespace Survey.Services
{
    public interface IVoteService
    {
        Task<Result> AddAsync(int pollid , string userid , VoteRequest voteRequest , CancellationToken cancellationToken);
    }
}
