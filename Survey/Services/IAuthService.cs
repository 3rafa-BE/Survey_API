using Survey.Abstractions;
using Survey.Contracts.Auth;

namespace Survey.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string email , string password , CancellationToken cancellationToken);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RevokeRefreshToken(string Token ,  string refreshToken, CancellationToken cancellationToken);
    }
}
