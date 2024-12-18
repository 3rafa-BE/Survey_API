namespace Survey.Contracts.Result
{
    public record VoteResponse(string VotterName , DateTime VoteDate , IEnumerable<QuestionAnswerRespone> QuestionAnswerRespones);
}
