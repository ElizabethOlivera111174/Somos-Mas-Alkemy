using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Helper.S3
{
    public class S3AwsHelper
    {
        private readonly IAmazonS3 _amazonS3;
        public S3AwsHelper()
        {
            var chain = new CredentialProfileStoreChain("App_data\\credentials.ini");
            AWSCredentials awsCredentials;
            RegionEndpoint uSEast1 = RegionEndpoint.USEast1;
            if (chain.TryGetAWSCredentials("default", out awsCredentials))
            {
                _amazonS3 = new AmazonS3Client(awsCredentials.GetCredentials().AccessKey, awsCredentials.GetCredentials().SecretKey, uSEast1);
            }
        }
        public async Task<AwsManagerResponse> AwsUploadFile(string key, IFormFile file)
        {
            try
            {
                var putRequest = new PutObjectRequest()
                {
                    BucketName = "cohorte-septiembre-91ddd87b",
                    Key = key,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };
                var result = await _amazonS3.PutObjectAsync(putRequest);
                var response = new AwsManagerResponse
                {
                    Message = "File upload successfully",
                    Code = (int)result.HttpStatusCode,
                    NameImage = key,
                    Url = $"https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/{key}"
                };
                return response;
            }
            catch (AmazonS3Exception e)
            {
                return new AwsManagerResponse
                {
                    Message = "Error encountered when writing an object",
                    Code = (int)e.StatusCode,
                    Errors = e.Message
                };
            }
            catch (Exception e)
            {
                return new AwsManagerResponse
                {
                    Message = "Unknown encountered on server when writing an object",
                    Code = 500,
                    Errors = e.Message
                };
            }
        }
        public async Task<AwsManagerResponse> AwsFileDelete(string key)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = "cohorte-septiembre-91ddd87b",
                    Key = key
                };
                var result = await _amazonS3.DeleteObjectAsync(request);
                var response = new AwsManagerResponse
                {
                    Message = "File deleted successfully",
                    Code = (int)result.HttpStatusCode,
                    NameImage = key
                };
                return response;
            }
            catch (AmazonS3Exception e)
            {
                return new AwsManagerResponse
                {
                    Message = "Error encountered when deleting an object",
                    Code = (int)e.StatusCode,
                    Errors = e.Message
                };
            }
            catch (Exception e)
            {
                return new AwsManagerResponse
                {
                    Message = "Unknown encountered on server when deleting an object",
                    Code = 500,
                    Errors = e.Message
                };
            }
        }
        public async Task<string> GetKeyFromUrl(string url)
        {
            string pattern = "https://alkemy-ong.s3.amazonaws.com/";
            string key = url.Substring(pattern.Length);
            return key;
        }
        public async Task<AwsManagerResponse> AwsGetFileUrl(string key)
        {
            try
            {
                var request = new GetObjectRequest()
                {
                    BucketName = "cohorte-septiembre-91ddd87b",
                    Key = key
                };
                using GetObjectResponse response = await _amazonS3.GetObjectAsync(request);
                var result = new AwsManagerResponse
                {
                    Message = "File encountered successfully",
                    Code = 200,
                    NameImage = response.Key,
                    Url = $"https://alkemy-ong.s3.amazonaws.com/{response.Key}"
                };
                return result;
            }
            catch (AmazonS3Exception e)
            {
                return new AwsManagerResponse
                {
                    Message = "Error encountered when writing an object",
                    Code = (int)e.StatusCode,
                    Errors = e.Message
                };
            }
            catch (Exception e)
            {
                return new AwsManagerResponse
                {
                    Message = "Unknown encountered on server when writing an object",
                    Code = 500,
                    Errors = e.Message
                };
            }
        }
    }
}
