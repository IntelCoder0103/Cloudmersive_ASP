namespace Cloudmersive.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(string connectionName, string fileName, string filePath, byte[] fileData);
        Task<byte[]> DownloadFileAsync(string connectionName, string fileName, string filePath);
        Task<bool> DeleteFileAsync(string connectionName, string fileName, string filePath);
    }
}
