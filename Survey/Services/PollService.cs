using Azure.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Contracts.Poll;
using Survey.Contracts.Question;
using Survey.Models;
using Survey.Persestance;

namespace Survey.Services
{

    public class PollService(DBContext dBContext) : IPollServices
    {
        private readonly DBContext _dbContext = dBContext;  
        public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _dbContext.polls.AsNoTracking().ProjectToType<PollResponse>().ToListAsync(cancellationToken);
        public async Task<IEnumerable<PollResponse>> GetAllCurrentAsync(CancellationToken cancellationToken = default) =>
            await _dbContext.polls.AsNoTracking()
            .Where(x=>x.IsPublished ==  true 
            && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) 
            && DateOnly.FromDateTime(DateTime.UtcNow) <= x.EndsAt)
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);
        public async Task<Result<PollResponse>> GetAsync(int id , CancellationToken cancellationToken) 
        {
            var poll = await _dbContext.polls.FindAsync(id);
            return poll is not null ?
                Result.Success(poll.Adapt<PollResponse>()):
                Result.Failure<PollResponse>(PollErrors.PollNotFound);
        } 
        
        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken)
        {
            //var isExistedTittle = await _dbContext.polls.AnyAsync(x => x.Title == request.Title);
            //if (isExistedTittle)
            //   return Result.Failure<PollResponse>(DuplicateTittleErrors.DuplicatedTittle);
            //var poll = request.Adapt<PollRequest>();
            //await _dbContext.AddAsync(poll , cancellationToken);
            //await _dbContext.SaveChangesAsync(cancellationToken);
            //return Result.Success(poll.Adapt<PollResponse>());
            var isExistingTitle = await _dbContext.polls.AnyAsync(x => x.Title == request.Title, cancellationToken: cancellationToken);

            if (isExistingTitle)
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPoll);

            var poll = request.Adapt<Poll>();

            await _dbContext.AddAsync(poll, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(poll.Adapt<PollResponse>());
        }

        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken)
        {
            var isExistedTittle = await _dbContext.polls.AnyAsync(x => x.Title == request.Title && x.Id != request.Id);
            if (isExistedTittle)
                return Result.Failure<PollResponse>(DuplicateTittleErrors.DuplicatedTittle);
            var currentPoll = await _dbContext.polls.FindAsync(id);
            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);
            var poll = request.Adapt<Poll>();
            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id , CancellationToken cancellationToken)
        {
            var poll =  await _dbContext.polls.FindAsync(id);

            if (poll is null)
                return Result.Failure(PollErrors.PollNotFound);

            _dbContext.polls.Remove(poll);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> TogglePublishedAsync(int id, CancellationToken cancellationToken)
        {
            var currentPoll = await _dbContext.polls.FindAsync(id);

            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);

            currentPoll.IsPublished = !currentPoll.IsPublished;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
