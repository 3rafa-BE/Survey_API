using Microsoft.EntityFrameworkCore;

namespace Survey.Models
{
    [Owned]
    public class RedreshToken
    {
        public string Token {  get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => RevokedOn is null && !IsExpired;
    }
}
