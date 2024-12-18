using Microsoft.AspNetCore.Identity;
using Survey.Abstractions;
using Survey.Contracts.Auth;
using Survey.Models;
using System.Security.Cryptography;

namespace Survey.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvidor jwtProvidor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IJwtProvidor jwtProvidor = jwtProvidor;
        private readonly int _refreshTokenExpiration = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
        {
            //Check for email
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
            //Check for password
            var IsValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!IsValidPassword)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
            //Generate Token
            var (token, expiresin) = jwtProvidor.GenerateToker(user);
            //Generate Refresh Token
            var refreshtoken = GenerateRefreshToken();
            //Generate ExpirationDate
            var expirationDate = DateTime.UtcNow.AddDays(_refreshTokenExpiration);
            //save To dataBase 
            user.RefreshTokens.Add(new RedreshToken { Token = refreshtoken, ExpiresOn = expirationDate });
            //Update user manger 
            await userManager.UpdateAsync(user);
            var respone = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresin, refreshtoken, expirationDate);
            return Result.Success(respone);
        }
        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
        {
            //check for the valid token 
            var userId = jwtProvidor.ValidateToke(token);
            if (userId is null) return Result.Failure<AuthResponse>(TokenErrors.EmptyToken);
            //check for the id
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Result.Failure<AuthResponse>(TokenErrors.EmptyToken);
            //check for the refresh token 
            var UserrefreshToken = user.RefreshTokens?.FirstOrDefault(x=>x.Token == refreshToken && x.IsActive);
            if (UserrefreshToken == null) return Result.Failure<AuthResponse>(TokenErrors.EmptyToken);
            //make it valid for one time 
            UserrefreshToken.RevokedOn = DateTime.UtcNow;
            //Make the creation of both refreshtoken and token
            var (newtoken, expiresin) = jwtProvidor.GenerateToker(user);
            //Generate Refresh Token
            var newrefreshtoken = GenerateRefreshToken();
            //Generate ExpirationDate
            var refreshtokenexpirationDate = DateTime.UtcNow.AddDays(_refreshTokenExpiration);
            //save To dataBase 
            user.RefreshTokens.Add(new RedreshToken { Token = newrefreshtoken, ExpiresOn = refreshtokenexpirationDate });
            //Update user manger 
            await userManager.UpdateAsync(user);
            var result = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, newtoken, expiresin, newrefreshtoken, refreshtokenexpirationDate);
            return Result.Success(result);
        }

        public async Task<Result> RevokeRefreshToken(string Token, string refreshToken, CancellationToken cancellationToken)
        {
            //check for the valid token 
            var userId = jwtProvidor.ValidateToke(Token);
            if (userId is null) return Result.Failure(TokenErrors.EmptyToken);
            //check for the id
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Result.Failure(TokenErrors.EmptyToken);
            //check for the refresh token 
            var UserrefreshToken = user.RefreshTokens?.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (UserrefreshToken == null) return Result.Failure(TokenErrors.EmptyToken);
            //make it valid for one time 
            UserrefreshToken.RevokedOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
            return Result.Success();
        }
        private static string GenerateRefreshToken()
        {
             return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
