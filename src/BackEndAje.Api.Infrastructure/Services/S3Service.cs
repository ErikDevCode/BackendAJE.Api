namespace BackEndAje.Api.Infrastructure.Services
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.S3.Transfer;
    using BackEndAje.Api.Application.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _region;

        public S3Service(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWS");

            this._bucketName = awsOptions["BucketName"]!;
            var accessKey = awsOptions["AccessKey"];
            var secretKey = awsOptions["SecretKey"];
            var region = awsOptions["Region"];
            this._region = awsOptions["Region"];

            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region),
            };

            this._s3Client = new AmazonS3Client(accessKey, secretKey, s3Config);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileFolder, string orderRequest, string fileName)
        {
            var key = $"{fileFolder}/{orderRequest}/{fileName}";
            return await this.UploadToS3(fileStream, key);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileFolder, string fileName)
        {
            var key = $"{fileFolder}/{fileName}";
            return await this.UploadToS3(fileStream, key);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileFolder, string clientId, string monthPeriod, string fileName)
        {
            var key = $"{fileFolder}/{clientId}/{monthPeriod}/{fileName}";
            return await this.UploadToS3(fileStream, key);
        }

        public async Task<bool> DeleteFileAsync(string key)
        {
            try
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = this._bucketName,
                    Key = key,
                };

                await this._s3Client.DeleteObjectAsync(deleteRequest);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error en el servidor. Message: '{ex.Message}'");
            }
            catch (Exception ex)
            {
                throw new Exception($"No encontrado error en el servidor. Message: '{ex.Message}'");
            }
        }

        private async Task<string> UploadToS3(Stream fileStream, string key)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(this._s3Client);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    Key = key,
                    BucketName = this._bucketName,
                };

                await fileTransferUtility.UploadAsync(uploadRequest);

                return $"https://{this._bucketName}.s3.{this._region}.amazonaws.com/{key}";
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error en el servidor. Message: '{ex.Message}'");
            }
            catch (Exception ex)
            {
                throw new Exception($"No encontrado error en el servidor. Message: '{ex.Message}'");
            }
        }
    }
}

