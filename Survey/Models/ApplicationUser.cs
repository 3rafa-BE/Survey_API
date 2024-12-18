using Microsoft.AspNetCore.Identity;

namespace Survey.Models
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName {  get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<RedreshToken> RefreshTokens { get; set; } = [];

    }
}
