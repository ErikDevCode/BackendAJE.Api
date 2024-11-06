using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IAssetRepository
    {
        Task<List<Asset>> GetAssets(int pageNumber, int pageSize);
        Task<int> GetTotalAssets();
        Task<Asset> GetAssetById(int assetId);
        
        Task<List<Asset>> GetAssetByCodeAje(string codeAje);
        Task AddAsset(Asset asset);
        Task AddAssetsAsync(IEnumerable<Asset> assets);
        Task UpdateAssetAsync(Asset asset);
        
        Task<Asset?> GetAssetByCodeAjeAndLogoAndAssetType(string codeAje, string logo, string? assetType);
        
        Task AddClientAsset(ClientAssets clientAssets);
        
        Task<List<ClientAssets>> GetClientAssetsByCodeAje(string codeAje);

        Task<List<ClientAssetsDto>> GetClientAssetsAsync(string? codeAje, int? clientId);
        Task<ClientAssets> GetClientAssetByIdAsync(int Id);
        
        Task UpdateClientAssetsAsync(ClientAssets clientAssets);
    }
}

