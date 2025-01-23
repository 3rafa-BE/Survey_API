using Survey.Abstractions;
using Survey.Contracts.Auth;
using Survey.Contracts.Register;

namespace Survey.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string email , string password , CancellationToken cancellationToken);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RevokeRefreshToken(string Token ,  string refreshToken, CancellationToken cancellationToken);
        Task<Result> RegisterAsync(registerRequest request, CancellationToken cancellationToken);
        Task<Result> ConfirmationEmail(ConfirmEmailRequest request);
        Task<Result> ResendVerficationCode(ResendVerficationCodeRequest request); 
     }
}
