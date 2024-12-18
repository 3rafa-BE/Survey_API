using FluentValidation;

namespace Survey.Contracts.Vote
{
    public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
    {
        public VoteAnswerRequestValidator()
        {
            RuleFor(x => x.AnswerId).
                GreaterThan(0);
            RuleFor(x => x.QuestionId).
                GreaterThan(0);
        }
    }
}
