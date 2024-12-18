using FluentValidation;

namespace Survey.Contracts.Question
{
    public class QuestionValidation : AbstractValidator<QuestionRequest>
    {
        public QuestionValidation() 
        {
            RuleFor(x => x.content)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.Answers)
                .NotNull();

            RuleFor(x => x.Answers)
                .Must(x => x.Count() > 1)
                .WithMessage("Question should have 2 Answers at least")
                .When(x=>x.Answers != null);

            RuleFor(x => x.Answers)
                .Must(x => x.Distinct().Count() == x.Count())
                .WithMessage("Answers should be distinct")
                .When(x => x.Answers != null);
        }
    }
}
