namespace BackEndAje.Api.Application.Clients.Commands.UploadClient
{
    public class UploadClientsResult
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