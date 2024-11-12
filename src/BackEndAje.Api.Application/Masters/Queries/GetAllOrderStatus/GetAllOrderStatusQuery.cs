namespace BackEndAje.Api.Application.Masters.Queries.GetAllOrderStatus
{
    using MediatR;

    public record GetAllOrderStatusQuery(int? userId = null) : IRequest<List<GetAllOrderStatusResult>>;
}