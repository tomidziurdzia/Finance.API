using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceApp.Application.Contracts;
using FinanceApp.Application.Models.Token;
using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinanceApp.Infrastructure.Services;

public class AuthService(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings)
    : IAuthService
{
    private JwtSettings _jwtSettings { get; } = jwtSettings.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),  // El nombre del usuario
            new Claim("userId", user.Id),  // El ID del usuario
            new Claim("email", user.Email!)  // El correo electrónico del usuario
        };

        var tokenOriginal = _jwtSettings.Key;
        var tokenInverse = new string(tokenOriginal!.Reverse().ToArray());
        var tokenFinal = tokenOriginal + tokenInverse;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenFinal));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }

    
    public string GetSessionUser()
    {
        var username = _httpContextAccessor.HttpContext!.User?.Claims?
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        return username!;
    }
}