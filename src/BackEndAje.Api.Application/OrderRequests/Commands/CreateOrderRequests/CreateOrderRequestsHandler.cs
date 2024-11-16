using BackEndAje.Api.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateOrderRequestsHandler : IRequestHandler<CreateOrderRequestsCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper, IMastersRepository mastersRepository, IHubContext<NotificationHub> hubContext)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
            this._mastersRepository = mastersRepository;
            this._hubContext = hubContext;
        }

        public async Task<Unit> Handle(CreateOrderRequestsCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = this._mapper.Map<OrderRequest>(request);
            var status = await this._mastersRepository.GetAllOrderStatus();
            var statusInitial = status.FirstOrDefault(x => x.StatusName == "GENERADO") !.OrderStatusId;
            orderRequest.OrderStatusId = statusInitial;
            orderRequest.IsActive = true;
            await this._orderRequestRepository.AddOrderRequestAsync(orderRequest);

            var orderRequestStatusHistory = new OrderRequestStatusHistory
            {
                OrderRequestId = orderRequest.OrderRequestId,
                OrderStatusId = statusInitial,
                ChangeReason = null,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
            };
            await this._orderRequestRepository.AddOrderRequestStatusHistoryAsync(orderRequestStatusHistory);

            var orderRequestId = orderRequest.OrderRequestId;

            if (request.Documents == null || !request.Documents.Any())
            {
                return Unit.Value;
            }

            var orderRequestDocuments = this._mapper.Map<List<OrderRequestDocument>>(request.Documents);

            foreach (var document in orderRequestDocuments)
            {
                document.OrderRequestId = orderRequestId;
                document.IsActive = true;
                document.CreatedBy = request.CreatedBy;
                document.UpdatedBy = request.UpdatedBy;
                await this._orderRequestRepository.AddOrderRequestDocumentAsync(document);
            }

            if (orderRequest.Supervisor == null)
            {
                return Unit.Value;
            }

            var notificationMessage = $"Se genero una orden para el cliente : {orderRequest.ClientCode}";

            await this._hubContext.Clients.User(orderRequest.Supervisor.UserId.ToString())
                .SendAsync("ReceiveMessage", "Sistema", notificationMessage, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}