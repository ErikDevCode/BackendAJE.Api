namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    
    public interface IClientAssetRepository
    {
        Task AddClientAsset(ClientAssets clientAssets);
        
        Task<List<ClientAssets>> GetClientAssetsByCodeAje(string codeAje);

        Task<List<ClientAssetsDto>> GetClientAssetsAsync(int? pageNumber, int? pageSize, string? codeAje, int? clientId, int? userId);
        Task<int> GetTotalClientAssets(string? codeaje, int? clientId);
        Task<ClientAssets> GetClientAssetByIdAsync(int Id);
        
        Task<ClientAssets> GetClientAssetByClientIdAndAssetIdAndIsNotActivateAsync(int clientId, int assetId);
        
        Task UpdateClientAssetsAsync(ClientAssets clientAssets);

        Task DeleteClientAssetAsync(ClientAssets clientAssets);

        Task AddTraceabilityRecordAsync(ClientAssetsTrace clientAssetsTrace);
        
        Task<List<ClientAssetsTrace>> GetClientAssetTracesByAssetId(int pageNumber, int pageSize, int? assetId);

        Task<int> GetTotalClientAssetsTrace(int? assetId);
        
        Task<ClientAssets> GetClientAssetPendingApprovalByClientIdAndAssetIdAsync(int clientId, int assetId);
    }
}