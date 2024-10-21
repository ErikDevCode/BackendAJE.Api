namespace BackEndAje.Api.Application.Users.Queries.GetUsersByParam
{
    using MediatR;

    public record GetUsersByParamQuery(string? Param) : IRequest<List<GetUsersByParamResult>>;
}
