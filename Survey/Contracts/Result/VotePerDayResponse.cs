namespace Survey.Contracts.Result
{
    public record VotePerDayResponse(DateOnly Day , int NumberOfVotes);
}
