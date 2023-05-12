using System.Collections;

namespace Cloudmersive.Services
{
    public class StorageService: IStorageService
    {
        // All registered storage services
        private readonly IDictionary<string, IBlobStorageService> storages = new Dictionary<string, IBlobStorageService>();
        public StorageService(AzureBlobStorageService azureBlobStorageService,
            AWSS3BlobStorageService awsS3BlobStorageService)
        {
            storages.Add("Azure", azureBlobStorageService);
            storages.Add("AWS", awsS3BlobStorageService);
        }
        public async Task<string> UploadFileAsync(string connectionName, string fileName, string filePath, byte[] fileData)
        {
            IBlobStorageService storageService;
            if(storages.TryGetValue(connectionName, out storageService))
            {
                return await storageService.UploadFileAsync(fileName, filePath, fileData);
            }
            return null;
        }
        public async Task<byte[]> DownloadFileAsync(string connectionName, string fileName, string filePath)
        {
            IBlobStorageService storageService;
            if (storages.TryGetValue(connectionName, out storageService))
            {
                return await storageService.DownloadFileAsync(fileName, filePath);
            }
            return null;
        }
        public async Task<bool> DeleteFileAsync(string connectionName, string fileName, string filePath)
        {
            IBlobStorageService storageService;
            if (storages.TryGetValue(connectionName, out storageService))
            {
                return await storageService.DeleteFileAsync(fileName, filePath);
            }
            return false;
        }
    }
}
