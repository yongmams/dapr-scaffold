using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.IO;
using System.Net;

namespace DapApp.Admin.API.Services
{
    public class S3FileService : IFileService
    {
        private readonly IAmazonS3 _client;

        public S3FileService(string endPoint, string accessKey, string secretKey)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = endPoint,
                ForcePathStyle = true
            };

            _client = new AmazonS3Client(credentials, config);
        }

        public async Task Upload(IFormFile file, string directoryName, string fileName)
        {
            var fileTransferUtility = new TransferUtility(_client);

            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = directoryName,
                Key = fileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead,
            };

            fileTransferUtilityRequest.TagSet = fileTransferUtilityRequest.TagSet ?? new List<Tag>();

            fileTransferUtilityRequest.TagSet.Add(new Tag() { Key = "origin", Value = file.FileName });

            await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
        }
    }
}
