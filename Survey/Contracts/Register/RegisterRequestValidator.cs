using FluentValidation;

namespace Survey.Contracts.Register
{
    public class RegisterRequestValidator : AbstractValidator<registerRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty();
                //Matches(Consts.PassWordPatteren);
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 100);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);
        }
        private class Consts
        {
            public const string PassWordPatteren = "@\"^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{6,}$\"";
        }
    }
}
