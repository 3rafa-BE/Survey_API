namespace Survey.Models
{
    public sealed class Question : AuditableEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Pollid {  get; set; }
        public Poll Poll { get; set; } = default!;
        public bool IsActive {  get; set; }
        public ICollection<Answer> Answers { get; set; } = [];
        public ICollection<VoteAnswers> Votes { get; set; } = [];
    }
}
