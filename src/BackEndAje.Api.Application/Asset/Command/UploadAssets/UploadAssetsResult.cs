namespace BackEndAje.Api.Application.Asset.Command.UploadAssets
{
    public class UploadAssetsResult
    {
        public bool Success { get; set; }

        public int ProcessedClients { get; set; }

        public List<UploadError> Errors { get; set; } = new();
    }

    public class UploadError
    {
        public int Row { get; set; }

        public string Message { get; set; }
    }
}