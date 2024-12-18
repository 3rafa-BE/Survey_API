using Survey.Abstractions;
using Survey.Contracts.Question;

namespace Survey.Services
{
    public interface IQuestionsServices
    {
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollid , CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollid , string UserId ,CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> GetByIdAsync(int pollid, int id , CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> AddAsync(int pollid, QuestionRequest request, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int pollid , int id , QuestionRequest request, CancellationToken cancellationToken = default); 
        Task<Result<QuestionResponse>> Togglestatue(int pollid, int id, CancellationToken cancellationToken);
    }
}
