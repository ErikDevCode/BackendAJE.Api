namespace BackEndAje.Api.Application.Users.Queries.GetUserById
{
    using MediatR;

    public record GetUserByIdQuery(int userId) : IRequest<GetUserByIdResult>;
}