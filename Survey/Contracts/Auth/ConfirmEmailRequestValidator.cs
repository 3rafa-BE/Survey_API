using FluentValidation;

namespace Survey.Contracts.Auth
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator() 
        {
            RuleFor(x=>x.code).NotEmpty();
            RuleFor(x=>x.userid).NotEmpty();
        }
    }
}
