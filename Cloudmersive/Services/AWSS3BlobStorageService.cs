using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Cloudmersive.Services
{
    public class AWSS3BlobStorageService : IBlobStorageService
    {
        private readonly AmazonS3Client amazonS3Client;
        private readonly TransferUtility transferUtility;
        private readonly string bucketName;

        public AWSS3BlobStorageService(IConfiguration configuration)
        {
            var awsOption = configuration.GetSection("ConnectionStrings:AWS");
            var accessKey = awsOption.GetValue<string>("AccessKey");
            var secretKey = awsOption.GetValue<string>("SecretKey");
            var region = Amazon.RegionEndpoint.GetBySystemName(awsOption.GetValue<string>("Region"));

            this.amazonS3Client = new AmazonS3Client(accessKey, secretKey, region);
            this.transferUtility = new TransferUtility(amazonS3Client);
            this.bucketName = "cloudmersivfiles";
        }
        public async Task<bool> DeleteFileAsync(string fileName, string filePath)
        {
            string key = $"{filePath}/{fileName}";
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };
            var res = await this.amazonS3Client.DeleteObjectAsync(request);
            return (int)res.HttpStatusCode >= 200 && (int)res.HttpStatusCode < 300;
        }

        public async Task<byte[]> DownloadFileAsync(string fileName, string filePath)
        {
            string key = $"{filePath}/{fileName}";
            var request = new TransferUtilityOpenStreamRequest
            {
                BucketName = bucketName,
                Key = key
            };
            var stream = await transferUtility.OpenStreamAsync(request);
            using(var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
            return null;
        }

        public async Task<string> UploadFileAsync(string fileName, string filePath, byte[] fileData)
        {
            string key = $"{filePath}/{fileName}";

            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = new MemoryStream(fileData)
            };
            await transferUtility.UploadAsync(request);

            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(5)
            };
            return new Uri(amazonS3Client.GetPreSignedURL(urlRequest)).AbsolutePath;
        }
    }
}
