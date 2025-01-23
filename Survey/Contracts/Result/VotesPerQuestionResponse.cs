namespace Survey.Contracts.Result
{
    public record VotesPerQuestionResponse(string Question , IEnumerable<VotesPerAnswerResponse> Votes);
}
