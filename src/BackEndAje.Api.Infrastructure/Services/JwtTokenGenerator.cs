namespace BackEndAje.Api.Infrastructure.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using BackEndAje.Api.Application.Dtos.JwtToken;
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public ResponseToken GeneratorToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(roles.Select(role => new Claim("role", role)));
            claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

            var expirationMinutes = int.Parse(this._configuration["JwtConfig:ExpirationInMinutes"]!);
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JwtConfig:Secret"] !));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: this._configuration["JwtConfig:Issuer"],
                audience: this._configuration["JwtConfig:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var tokenExpiresUnix = new DateTimeOffset(expiration).ToUnixTimeSeconds();
            var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timeRemainingInSeconds = tokenExpiresUnix - currentUnixTime;

            return new ResponseToken
            {
                Token = tokenString,
                TokenExpiresSeg = timeRemainingInSeconds,
            };
        }
    }
}
