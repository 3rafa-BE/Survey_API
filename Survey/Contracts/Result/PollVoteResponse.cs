namespace Survey.Contracts.Result
{
    public record PollVoteResponse(string Tittle , IEnumerable<VoteResponse> VoteResponses);
}
