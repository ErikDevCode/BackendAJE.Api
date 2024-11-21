namespace BackEndAje.Api.Application.OrderRequestAssets.Commands.AssignAssetsToOrderRequest
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class AssignAssetsToOrderRequestCommand : IRequest<Unit>, IHasAssignedBy
    {
        public int OrderRequestId { get; set; }

        public List<int> AssetIds { get; set; }

        public int AssignedBy { get; set; }
    }
}