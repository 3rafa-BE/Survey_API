namespace Survey.Contracts.Vote
{
    public record VoteAnswerRequest(
        int QuestionId ,
        int AnswerId
        );
}
