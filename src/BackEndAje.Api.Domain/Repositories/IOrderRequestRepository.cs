using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IOrderRequestRepository
    {
        Task AddOrderRequestAsync(OrderRequest orderRequest);
        
        Task AddOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task<OrderRequest?> GetOrderRequestById(int id);

        Task<List<OrderRequestDocument>> GetOrderRequestDocumentByOrderRequestId(int id);
        
        Task<OrderRequestDocument> GetOrderRequestDocumentById(int id);
        
        Task DeleteOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);

        Task<List<OrderRequest>> GetAllOrderRequestAsync(
            int? pageNumber,
            int? pageSize,
            int? clientCode,
            int? orderStatusId,
            int? reasonRequestId,
            int? CediId,
            int? RegionId,
            DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null);

        Task<int> GetTotalOrderRequestCountAsync(
            int? clientCode,
            int? orderStatusId,
            int? reasonRequestId,
            int? CediId,
            int? RegionId,
            DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null);
        
        Task DeleteOrderRequestAsync(OrderRequest orderRequest);
        
        Task AddOrderRequestStatusHistoryAsync(OrderRequestStatusHistory orderRequestStatusHistory);

        Task UpdateStatusOrderRequestAsync(int orderRequestId, int newStatusId, int createdBy);
        
        Task<List<OrderRequestStatusHistory>> GetOrderRequestStatusHistoryByOrderRequestId(int orderRequestId);
        
        Task UpdateOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);

        Task<int> GetTotalOrderRequestStatusCount(
            int? statusId = null,
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null);

        Task<int> GetTotalOrderRequestReasonCount(
            int? reasonRequestId = null,
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null);
        
        Task<int> GetTotalAssetFromOrderRequestStatusAttendedCount(
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null);
        
        Task<int> AssignAssetToOrder(int orderRequestId, int assetId, int assignedBy);

        Task AddOrderRequestAssetTrace(OrderRequestAssetsTrace orderRequestAssetsTrace);
        Task<OrderRequestAssets> GetOrderRequestAssetsById(int orderRequestAssetId);
        
        Task UpdateAssetToOrderRequest(OrderRequestAssets orderRequestAssets);
        
        Task<List<OrderRequestAssetsTrace>> GetOrderRequestAssetsTraceByOrderRequestId(int orderRequestId);
        
        Task BulkInsertOrderRequestsAsync(IEnumerable<OrderRequest> orderRequests);
        
        Task<bool> ExistsAsync(int reasonRequestId, int clientId, DateTime negotiatedDate);
        
        Task<List<OrderRequest>> GetAllAsync(
            int? clientCode,
            int? orderStatusId,
            int? reasonRequestId,
            int? CediId,
            int? RegionId,
            DateTime? startDate,
            DateTime? endDate,
            int? supervisorId = null,
            int? vendedorId = null);

        Task AddRelocation(Relocation relocation);
        Task AddRelocationRequests(RelocationRequest relocationRequest);
        
        Task<RelocationRequest> GetRelocationRequestByOrderRequestId(int orderRequestId);
        
        Task UpdateRelocationRequest(RelocationRequest relocationRequest);
        
        Task<List<RelocationRequest>> GetListRelocationRequestByOrderRequestId(int orderRequestId);
        
        Task DeleteRelocationAsync(Relocation relocation);
        Task DeleteRelocationRequestAsync(RelocationRequest relocationRequest);
        
        Task<Relocation> GetRelocationById(int relocationId);
        
        Task<List<RelocationRequest>> GetRelocationRequestByRelocationId(int relocationId);
    }
}
