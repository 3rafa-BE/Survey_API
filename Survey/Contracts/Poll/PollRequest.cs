namespace Survey.Contracts.Poll
{
    public record PollRequest(int id, string Title, string Summary, DateOnly StartsAt, DateOnly EndsAt);
}
