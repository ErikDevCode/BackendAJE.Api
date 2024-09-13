namespace BackEndAje.Api.Application.Interfaces
{
    using BackEndAje.Api.Application.Dtos.JwtToken;
    using BackEndAje.Api.Domain.Entities;

    public interface IJwtTokenGenerator
    {
        ResponseToken GeneratorToken(User user, IEnumerable<string> roles, IEnumerable<Role> rolesWithPermissions);
    }
}
