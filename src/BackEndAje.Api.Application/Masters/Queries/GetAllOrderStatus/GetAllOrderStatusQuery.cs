namespace BackEndAje.Api.Application.Masters.Queries.GetAllOrderStatus
{
    using MediatR;

    public record GetAllOrderStatusQuery : IRequest<List<GetAllOrderStatusResult>>;
}