using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contracts.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.Identity;

namespace Infrastructure.Identity;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
    }

    public TokenResponse GetToken(TokenRequest request)
    {
        // var claims = new List<Claim>
        // {
        //     new Claim(ClaimTypes.Name, request.Email)
        // };
        var token = GenerateJwt();
        return new TokenResponse(token);
    }

    private string GenerateJwt() => GenerateEncryptedToken(GetSigningCredentials());

    private string GenerateEncryptedToken(SigningCredentials signingCredentials)
    {
        var claims = new List<Claim>
        {
            new Claim("Role", "Admin")
        };
        var token = new JwtSecurityToken(
            claims: claims,
            // expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}