using System.ComponentModel.DataAnnotations;

namespace Survey.Contracts.Auth
{
    public class JwtOptions
    {
        public static string SectionName = "JWT";
        [Required]
        public string Key { get; init; } = string.Empty;
        [Required]

        public string Issuer { get; init; } = string.Empty;
        [Required]

        public string Audience { get; init; } = string.Empty;
        [Range(1 ,30 , ErrorMessage ="Error happens on this field")]
        public int ExpiryMinutes { get; init; }
       
    }
}
