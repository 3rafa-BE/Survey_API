﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Survey.Abstractions;
using Survey.Contracts.Auth;
using Survey.Contracts.Register;
using Survey.Services;

namespace Survey.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController(IAuthService authService , IOptions<JwtOptions> JwtOptions) : ControllerBase
    {
        private readonly IAuthService authService = authService;
        private readonly JwtOptions jwtOptions = JwtOptions.Value;

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Loginrequest request, CancellationToken cancellationToken)
        {

            var authResult = await authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            //return authResult is null ? BadRequest() : Ok(authResult);
            return authResult.IsSuccess ?
               Ok() :
               authResult.ToProblem();
        }
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await authService.RevokeRefreshToken(request.Token, request.RefreshToken, cancellationToken);
            return authResult.IsSuccess ?
                Ok() :
                authResult.ToProblem();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]registerRequest request, CancellationToken cancellationToken)
        {
            var result = await authService.RegisterAsync(request,cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await authService.ConfirmationEmail(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("resend-code")]
        public async Task<IActionResult> ResendVerficationCode([FromBody] ResendVerficationCodeRequest request)
        {
            var result = await authService.ResendVerficationCode(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
