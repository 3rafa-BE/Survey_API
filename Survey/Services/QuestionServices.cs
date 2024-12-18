using Mapster;
using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Contracts.Question;
using Survey.Models;
using Survey.Persestance;

namespace Survey.Services
{
    public class QuestionServices(DBContext dBContext) : IQuestionsServices
    {
        private readonly DBContext _dBContext = dBContext;


        public async Task <Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollid, CancellationToken cancellationToken = default)
        {
            //check for the id
            var isExistedPoll = await _dBContext.polls.AnyAsync(x=>x.Id==pollid , cancellationToken : cancellationToken);
            if(!isExistedPoll)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            //retun questions with the answer
            var questions = await _dBContext.questions.Where(x => x.Pollid == pollid)
                .Include(x=>x.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .ToListAsync();
            return Result.Success<IEnumerable<QuestionResponse>>(questions);
        }
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollid, string UserId, CancellationToken cancellationToken = default)
        {
            var HasVoted = await _dBContext.votes.AnyAsync(x => x.pollid == pollid && x.UserId == UserId, cancellationToken);
            if(HasVoted)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteError.DuplicatedVoting);
            var pollExists = await _dBContext.polls.AnyAsync(x=>x.Id == pollid 
            && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
            && DateOnly.FromDateTime(DateTime.UtcNow) <= x.EndsAt);
            if(!pollExists)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            var questions = await _dBContext.questions
                .Where(x=>x.Pollid == pollid && x.IsActive)
                .Include(x=>x.Answers)
                .Select(a=> new QuestionResponse(
                    a.Id,
                    a.Content,
                    a.Answers.Select(a=>new Contracts.Answers.AnswerResponse(
                        a.Id,a.Content
                        ))
                    )).ToListAsync(cancellationToken);
            return Result.Success<IEnumerable<QuestionResponse>>(questions);
        }
        public async Task<Result<QuestionResponse>> GetByIdAsync(int pollid, int id, CancellationToken cancellationToken = default)
        {
            //check for the questions and poll id
            var question = await _dBContext.questions
                .Where(x=> x.Pollid == pollid && x.Id == id)
                .Include(x=>x.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
                 if (question is null)
                     return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
                 return Result.Success<QuestionResponse>(question);
        }
        public async Task<Result<QuestionResponse>> AddAsync(int pollid, QuestionRequest request, CancellationToken cancellationToken)
        {
            //check the poll id
            var isvaildpollid = await _dBContext.polls.AnyAsync(x=>x.Id == pollid , cancellationToken);
            if (!isvaildpollid)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            //check the duplicate value
            var isvalidQuestion = await _dBContext.questions.AnyAsync(x=>x.Content == request.content && x.Id == pollid , cancellationToken);
            if(!isvalidQuestion)
                return Result.Failure<QuestionResponse>(DuplicateQuestionContent.DuplicatedContent);
            // adding answers 
            var question = request.Adapt<Question>();
            question.Id = pollid;
            await _dBContext.questions.AddAsync(question , cancellationToken);
            await _dBContext.SaveChangesAsync(cancellationToken);
            return Result.Success(question.Adapt<QuestionResponse>());
        }

        public async Task<Result<QuestionResponse>> Togglestatue(int pollid, int id, CancellationToken cancellationToken)
        {
            var question = await _dBContext.questions.FirstOrDefaultAsync(x => x.Pollid == pollid && x.Id == id);
            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
            question.IsActive = !question.IsActive;
            await _dBContext.SaveChangesAsync(cancellationToken);
            return Result.Success(question.Adapt<QuestionResponse>());
        }

        public async Task<Result> UpdateAsync(int pollid, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            //check the duplication
            var QuestionIsExisited = await _dBContext.questions.
                AnyAsync(x => x.Id != id && x.Pollid == pollid && x.Content == request.content, cancellationToken);
            if (QuestionIsExisited)
                return Result.Failure(DuplicateQuestionContent.DuplicatedContent);
            //get the question 
            var question = await _dBContext.questions
                .Include(x=>x.Answers)
                .SingleOrDefaultAsync(x => x.Id == id && x.Pollid == pollid , cancellationToken);
            if (question is null)
                return Result.Failure(QuestionError.QuestionNotFound);
            //modification 
            question.Content = request.content;
            //getting the current answers
            var currentanswers = question.Answers.Select(x=>x.Content).ToList();
            //getting the new answers 
            var newAnswers = request.Answers.Except(currentanswers).ToList();
            //Adding the new anwers
            newAnswers.ForEach(answer =>
            {
                question.Answers.Add(new Answer { Content = answer });
            });
            //changing the statue of the 
            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });
            await _dBContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
