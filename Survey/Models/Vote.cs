namespace Survey.Models
{
    public sealed class Vote
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int pollid { get; set; }
        public DateTime SubmittedOn { get; set; }
        public Poll Poll { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<VoteAnswers> VoteAnswers { get; set; } = [];
    }
}
