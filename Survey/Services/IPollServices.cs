using Survey.Abstractions;
using Survey.Contracts.Poll;
using Survey.Models;
namespace Survey.Services
{
    public interface IPollServices
    {
         Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PollResponse>> GetAllCurrentAsync(CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> GetAsync(int id , CancellationToken cancellationToken = default);
         Task<Result<PollResponse>> AddAsync(PollRequest request , CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id , CancellationToken cancellationToken);
        Task<Result> TogglePublishedAsync(int id , CancellationToken cancellationToken);
    }
}
