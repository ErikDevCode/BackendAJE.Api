namespace BackEndAje.Api.Application.Clients.Commands.UploadDeleteClients
{
    public class UploadDeleteClientsResult
    {
        public bool Success { get; set; }

        public int UpdatedCount { get; set; }

        public List<UploadError> Errors { get; set; } = new();
    }

    public class UploadError
    {
        public int Row { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}

