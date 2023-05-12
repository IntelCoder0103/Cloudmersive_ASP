using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Cloudmersive.Services
{
    public class AzureBlobStorageService : IBlobStorageService
    {
        private readonly string _azureConnectionString;
        private readonly string _bucketName = "chat";
        private readonly CloudBlobContainer _cloudBlobContainer;
        public AzureBlobStorageService(IConfiguration configuration)
        {
            this._azureConnectionString = configuration.GetConnectionString("Azure");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(this._azureConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference(_bucketName);
        }

        public async Task<string> UploadFileAsync(string fileName, string filePath, byte[] fileData)
        {
            string fileKey = $"{filePath}/{fileName}";
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(fileKey);

            await blockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);

            return blockBlob.Uri.AbsoluteUri;

        }

        public async Task<bool> DeleteFileAsync(string fileName, string filePath)
        {
            string fileKey = $"{filePath}/{fileName}";

            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(fileKey);

            await blockBlob.DeleteAsync();

            return true;
        }

        public async Task<byte[]> DownloadFileAsync(string fileName, string filePath)
        {
            string fileKey = $"{filePath}/{fileName}";
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(fileKey);

            using (MemoryStream ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms);
                byte[] fileBytes = ms.ToArray();
                return fileBytes;
                // Do something with the fileBytes array
            }

            return null;
        }
    }
}
