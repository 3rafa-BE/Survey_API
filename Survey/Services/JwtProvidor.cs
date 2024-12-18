using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Survey.Contracts.Auth;
using Survey.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Survey.Services
{
    public class JwtProvidor(IOptions<JwtOptions> JwtOptions) : IJwtProvidor
    {
        private readonly JwtOptions jwtOptions = JwtOptions.Value;

        public (string Token, int Experiesin) GenerateToker(ApplicationUser user)
        {
            Claim[] claims = [
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub , user.Id),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email , user.Email!),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.GivenName , user.FirstName),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.FamilyName , user.LastName),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name , user.UserName)
                ];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires:DateTime.UtcNow.AddMinutes(jwtOptions.ExpiryMinutes),
                signingCredentials: sigingCredentials
                );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresin: jwtOptions.ExpiryMinutes);
        }

        public string? ValidateToke(string Token)
        {
            var tokenHandeler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            try 
            {
                tokenHandeler.ValidateToken(Token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                //convert from security Token to Jwt Security Token
                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
