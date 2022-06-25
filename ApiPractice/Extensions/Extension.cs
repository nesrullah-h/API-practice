using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiPractice.Extensions
{
    public static class Extention
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }

        public static bool CheckSize(this IFormFile file, int kb)
        {
            return file.Length / 1024 > kb;
        }

        public static async Task<string> SaveImage(this IFormFile formFile, IWebHostEnvironment envpath, string folder)
        {
            string path = envpath.WebRootPath;
            string filename = Guid.NewGuid().ToString() + formFile.FileName;
            string result = Path.Combine(path, folder, filename);
            using (FileStream fileStream = new FileStream(result, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return filename;
        }
    }
}
