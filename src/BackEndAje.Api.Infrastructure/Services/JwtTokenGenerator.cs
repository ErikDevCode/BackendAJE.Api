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
    using Newtonsoft.Json;

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public ResponseToken GeneratorToken(User user, IEnumerable<string> roles, IEnumerable<Role> rolesWithPermissions)
        {
            var email = user.Email ?? "N/A";
            var route = user.Route?.ToString() ?? "N/A";

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Nickname, route),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(rolesWithPermissions.Select(role => new Claim("role", role.RoleName)));

            var permissionsDict = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();

            foreach (var role in rolesWithPermissions)
            {
                var rolePermissions = new Dictionary<string, Dictionary<string, bool>>();

                foreach (var rp in role.RolePermissions.Where(rp => rp.Status))
                {
                    if (!rolePermissions.ContainsKey(rp.Permission.PermissionName))
                    {
                        rolePermissions[rp.Permission.PermissionName] = new Dictionary<string, bool>();
                    }

                    rolePermissions[rp.Permission.PermissionName][rp.Permission.Action] = true;
                }

                permissionsDict[role.RoleName] = rolePermissions;
            }

            var permissionsJson = JsonConvert.SerializeObject(permissionsDict);
            claims.Add(new Claim("permissions", permissionsJson));

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
