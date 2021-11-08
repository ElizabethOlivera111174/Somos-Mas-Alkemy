using Microsoft.AspNetCore.Http;
using System.IO;

namespace OngProject.Core.Helper.FomFileData
{
    public class ConvertFile
    {
        public static IFormFile BinaryToFormFile(byte[] bytes, FormFileData formFileData)
        {
            MemoryStream stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream, 0, bytes.Length, formFileData.Name, formFileData.FileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = formFileData.ContentType
            };
            return file;
        }
    }
}
