namespace BackEndAje.Api.Application.Asset.Command.UploadClientAssets
{

    public class UploadClientAssetResult
    {
        public bool Success { get; set; }

        public int ProcessedAssets { get; set; }

        public List<UploadError> Errors { get; set; } = new List<UploadError>();
    }
}
