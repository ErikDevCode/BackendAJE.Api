namespace BackEndAje.Api.Application.Clients.Queries.GetAllClients
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllClientsQuery (int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<GetAllClientsResult>>;
}
