using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibraryMVC.Utils
{
    public interface IImageReader
    {
        Task<string> ConvertToBase64Async(IFormFile file);
    }

    public class ImageReader : IImageReader
    {
        public async Task<string> ConvertToBase64Async(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}