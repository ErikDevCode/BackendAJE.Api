namespace BackEndAje.Api.Application.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(Stream fileStream, string file, string orderRequest, string fileName);

        Task<string> UploadFileAsync(Stream fileStream, string file, string fileName);

        Task<bool> DeleteFileAsync(string key);
    }
}