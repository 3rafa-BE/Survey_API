using Microsoft.Extensions.Options;
using Survey.Contracts.Auth;
using Survey.Models;

namespace Survey.Services
{
    public interface IJwtProvidor
    {
        (string Token, int Experiesin) GenerateToker(ApplicationUser user);
        string? ValidateToke(string Token);
    }
}
