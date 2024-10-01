namespace BackEndAje.Api.Application.Users.Queries.GetMenuForUserById
{
    using MediatR;

    public record GetMenuForUserByIdQuery(int UserId) : IRequest<GetMenuForUserByIdResult>
    {
    }
}
