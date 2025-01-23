using FluentValidation;

namespace Survey.Contracts.Auth
{
    public class ResendVerficationCodeValidator : AbstractValidator<ResendVerficationCodeRequest>
    {
        public ResendVerficationCodeValidator()
        {
            RuleFor(x=>x.email).NotEmpty();
            RuleFor(x => x.email).EmailAddress();
        }
    }
}
