namespace Survey.Contracts.Vote
{
    public record VoteRequest(
        IEnumerable<VoteAnswerRequest> Answers
        );
}
