using FluentValidation;

namespace Survey.Contracts.Vote
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(x=>x.Answers).NotEmpty();
            RuleForEach(x => x.Answers)
                .SetInheritanceValidator(v => v.Add
                (new VoteAnswerRequestValidator()));
        }
    }
}
