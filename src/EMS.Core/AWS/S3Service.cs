using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using Microsoft.Extensions.Configuration;
using Amazon.S3.Model;
using System;

namespace EMS.Core.AWS
{
    public class S3Service : IS3Service
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;

        public S3Service(
            IConfiguration configuration,
            IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _configuration = configuration;
        }

        public async Task UploadFileToS3(IFormFile file, string key)
        {
            var s3 = _configuration["AWS:S3Bucket"];

            using (var newMemoryStream = new MemoryStream())
            {
                file.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = key,
                    BucketName = s3
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }
        }

        public string GenereatePresignedURL(string key)
        {
            var request1 = new GetPreSignedUrlRequest
            {
                BucketName = _configuration["AWS:S3Bucket"],
                Key = key,
                Expires = DateTime.Now.AddSeconds(GetExpiry())
            };

            return _s3Client.GetPreSignedURL(request1);
        }

        private int GetExpiry()
        {
            int expiry = 0;
            return Int32.TryParse(_configuration["AWS:PreSignedURLExpiryInSeconds"], out expiry)
                ? expiry
                : 60;
        }
    }
}