namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    public interface IAssetRepository
    {
        Task<List<Asset>> GetAssets(int pageNumber, int pageSize, string? codeAje);
        Task<int> GetTotalAssets(string? codeAje);
        Task<Asset> GetAssetById(int assetId);
        
        Task<List<Asset>> GetAssetByCodeAje(string codeAje);
        Task AddAsset(Asset asset);
        Task AddAssetsAsync(IEnumerable<Asset> assets);
        Task UpdateAssetAsync(Asset asset);
        
        Task<Asset?> GetAssetByCodeAjeAndLogoAndAssetType(string codeAje, string logo, string? assetType);
        
        Task<List<Asset>> GetAssetsWithOutClient(int pageNumber, int pageSize, string? codeAje);
        Task<int> GetTotalAssetsWithOutClient(string? codeAje);
    }
}

