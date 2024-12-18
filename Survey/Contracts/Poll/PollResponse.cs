namespace Survey.Contracts.Poll
{
    public record PollResponse(int id, string Title, string Summary, bool IsPublished, DateOnly StartsAt, DateOnly EndsAt);
}
