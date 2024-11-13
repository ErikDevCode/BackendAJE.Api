namespace BackEndAje.Api.Application.OrderRequestAssets.Commands.AssignAssetsToOrderRequest
{
    using MediatR;

    public class AssignAssetsToOrderRequestCommand : IRequest<Unit>
    {
        public int OrderRequestId { get; set; }

        public List<int> AssetIds { get; set; }

        public int AssignedBy { get; set; }
    }
}