using Survey.Abstractions;
using Survey.Contracts.Result;

namespace Survey.Services
{
    public interface IResultService
    {
        Task<Result<PollVoteResponse>> GetVotesAsync(int pollid, CancellationToken cancellationToken);
    }
}
