namespace BackEndAje.Api.Application.OrderRequestAssets.Commands.DeleteAssetToOrderRequest
{
    using MediatR;

    public class DeleteAssetToOrderRequestCommand : IRequest<Unit>
    {
        public int OrderRequestAssetId { get; set; }

        public int AssignedBy { get; set; }
    }
}