using Survey.Contracts.Answers;

namespace Survey.Contracts.Question
{
    public record QuestionResponse(int id , string content , IEnumerable<AnswerResponse> Answers);
}
