using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Contracts.Auth;
using Survey.Contracts.Register;
using Survey.Helpers;
using Survey.Models;
using System.Security.Cryptography;
using System.Text;

namespace Survey.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, 
        IJwtProvidor jwtProvidor , 
        SignInManager<ApplicationUser> signInManager
        ,ILogger<AuthService> logger
        ,IHttpContextAccessor httpContextAccessor
        ,IEmailSender emailSender) : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IJwtProvidor jwtProvidor = jwtProvidor;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly int _refreshTokenExpiration = 14;


        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
        {
            //Check for email
            //var user = await userManager.FindByEmailAsync(email);
            if (await userManager.FindByEmailAsync(email) is not { } user)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
            //Check for password
            //var IsValidPassword = await userManager.CheckPasswordAsync(user, password);
            //if (!IsValidPassword)
            //    return Result.Failure<AuthResponse>(UserErrors.InvalidCredentionals);
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
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
            ////Generate Token
            //var (token, expiresin) = jwtProvidor.GenerateToker(user);
            ////Generate Refresh Token
            //var refreshtoken = GenerateRefreshToken();
            ////Generate ExpirationDate
            //var expirationDate = DateTime.UtcNow.AddDays(_refreshTokenExpiration);
            ////save To dataBase 
            //user.RefreshTokens.Add(new RedreshToken { Token = refreshtoken, ExpiresOn = expirationDate });
            ////Update user manger 
            //await userManager.UpdateAsync(user);
            //var respone = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresin, refreshtoken, expirationDate);
            //return Result.Success(respone);
            return Result.Failure<AuthResponse>(
                result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentionals);
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

        public async Task<Result> RegisterAsync(registerRequest request, CancellationToken cancellationToken)
        {
            //check if email exists or not 
            var IsExits = await userManager.Users.AnyAsync(x => x.Email == request.Email);
            if (IsExits)
                return Result.Failure(UserErrors.DuplicateEmail);
            var user = request.Adapt<ApplicationUser>();
            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation("Confitmation Code {code}", code);
                await SendConfirmationEmail(user, code);
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure<AuthResponse>(new Error (error.Code ,error.Description , StatusCodes.Status409Conflict));
        }

        public async Task<Result> ConfirmationEmail(ConfirmEmailRequest request)
        {
            //get user using userid 
            if (await userManager.FindByIdAsync(request.userid) is not { } user)
                return Result.Failure(UserErrors.InvalidCredentionals);
            //check if email is confirmed or not 
            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicateConfirmation);
            var code = request.code;
            //make decoding for the code
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch(FormatException)
            {
                return Result.Failure(UserErrors.InvalidCredentionals);
            }
            //confirm the email
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) 
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendVerficationCode(ResendVerficationCodeRequest request)
        {
            if (await userManager.FindByEmailAsync(request.email) is not { } user)
                return Result.Success();
            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicateConfirmation);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confitmation Code {code}", code);
            await SendConfirmationEmail(user, code);
            return Result.Success();
        }
        
        private async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
                }
            );

            await _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confirmation", emailBody);
        }

    }
}
