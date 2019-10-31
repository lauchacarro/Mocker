using Microsoft.AspNetCore.Http;
using Mocker.Models.File;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class FileExtension
    {
        public static byte[] ConvertToBlob(this FileStream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            return bytes;
        }

        public static string ToBase64String(this FileStream stream)
        {
            return Convert.ToBase64String(stream.ConvertToBlob());
        }

        public static async Task<FileModel> ToFileModelAsync(this IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            byte[] bytes = stream.ConvertToBlob();

            FileModel fileModel = new FileModel
            {
                Name = file.FileName,
                Base64 = stream.ToBase64String(),
                ContentType = file.ContentType,
                Lenght = file.Length
            };

            stream.Close();

            if (File.Exists(filePath))
                File.Delete(filePath);

            return fileModel;
        }
    }
}
