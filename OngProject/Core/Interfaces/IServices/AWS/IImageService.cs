using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices.AWS
{
    public interface IImageService
    {
        Task<String> Save(string fileName, IFormFile image);

        Task<bool> Delete(string name);

    }
}
