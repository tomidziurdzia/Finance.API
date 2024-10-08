using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceApp.Application.Models.Token;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),
            new Claim("userId", user.Id),
            new Claim("email", user.Email!)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
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
}