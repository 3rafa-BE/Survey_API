namespace Survey.Contracts.Poll
{
    public record PollRequest(int Id, string Title, string Summary, DateOnly StartsAt, DateOnly EndsAt);
}
