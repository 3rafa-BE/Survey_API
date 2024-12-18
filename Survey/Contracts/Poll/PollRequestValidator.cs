using FluentValidation;

namespace Survey.Contracts.Poll
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Summary)
                .NotEmpty();

            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .LessThanOrEqualTo(x => x.EndsAt);

            RuleFor(x => x.EndsAt)
                .NotEmpty();

            RuleFor(x => x)
                .Must(HasValidDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} must be graeter than or equal start date");
        }
        private bool HasValidDate(PollRequest pollRequest)
        {
            return pollRequest.EndsAt <= pollRequest.StartsAt;
        }
    }
}
