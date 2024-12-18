using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace Survey.Contracts.Poll
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).
                EmailAddress().
                NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
