namespace BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason
{
    using MediatR;

    public record GetWithDrawalReasonQuery(int ReasonRequestId) : IRequest<List<GetWithDrawalReasonResult>>
    {
    }
}
