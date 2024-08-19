namespace BackEndAje.Api.Application.Users.Queries.GetUser
{
    using MediatR;

    public record GetUserQuery(string UserEmail) : IRequest<GetUserResult>;
}
