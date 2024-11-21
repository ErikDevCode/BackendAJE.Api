namespace BackEndAje.Api.Application.Users.Queries.GetMenuForUserById
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetMenuForUserByIdQuery() : IRequest<GetMenuForUserByIdResult>, IHasUserId
    {
        public int UserId { get; set; }
    }
}
