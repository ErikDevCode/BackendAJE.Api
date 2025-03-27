namespace BackEndAje.Api.Application.Clients.Queries.GetAllClients
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetAllClientsQuery(int PageNumber = 1, int PageSize = 10, string? Filtro = null) : IRequest<PaginatedResult<GetAllClientsResult>>, IHasUserId
    {
        public int UserId { get; set; }
    }
}
