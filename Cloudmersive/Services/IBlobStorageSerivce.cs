namespace Cloudmersive.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string fileName, string filePath, byte[] fileData);
        Task<byte[]> DownloadFileAsync(string fileName, string filePath);
        Task<bool> DeleteFileAsync(string fileName, string filePath);
    }
}
