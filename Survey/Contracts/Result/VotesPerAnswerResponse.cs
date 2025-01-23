namespace Survey.Contracts.Result
{
    public record VotesPerAnswerResponse(
        string Answer , 
        int NumberOfVotes
    );
}
