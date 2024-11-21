namespace BackEndAje.Api.Application.OrderRequestAssets.Commands.DeleteAssetToOrderRequest
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class DeleteAssetToOrderRequestCommand : IRequest<Unit>, IHasAssignedBy
    {
        public int OrderRequestAssetId { get; set; }

        public int AssignedBy { get; set; }
    }
}