using System;
using System.IO;

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
    }
}
