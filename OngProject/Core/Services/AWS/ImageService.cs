using Microsoft.AspNetCore.Http;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services.AWS
{
    public class ImageService : IImageService
    {
        private S3AwsHelper _s3AwsHelper;
        public ImageService()
        {
            _s3AwsHelper = new S3AwsHelper();
        }
        public async Task<String> Save(string fileName, IFormFile image)
        {
            AwsManagerResponse responseAws;
            if (image != null)
            {
                if (ValidateFiles.ValidateImage(image))
                {
                    responseAws = await _s3AwsHelper.AwsUploadFile(fileName, image);
                    if (!String.IsNullOrEmpty(responseAws.Errors))
                    {
                        throw new Exception("Error al guardar imagen. Detalle:" + responseAws.Errors);
                    }
                    return responseAws.Url;
                }
                else
                    throw new Exception("Extensión de imagen no válida. Debe ser jpg, png o jpeg.");
            }
            else
                throw new Exception("Error, no existe imagen.");
        }
        public async Task<bool> Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                string nameImage = await _s3AwsHelper.GetKeyFromUrl(name);
                AwsManagerResponse responseAws = await _s3AwsHelper.AwsFileDelete(nameImage);
                if (!String.IsNullOrEmpty(responseAws.Errors))
                    return false;
                return true;
            }
        }
    }
}
