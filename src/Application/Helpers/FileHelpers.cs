using DocConverter.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace DocConverter.Application.Helpers
{
    public static class FileHelpers
    {
        public static async Task SaveFileAsync(IFormFile formFile, string fileName, string directory)
        {
            await using var stream = File.Create(Path.Combine(directory, fileName));
            await formFile.CopyToAsync(stream);
            stream.Close();
        }


        public static async Task<byte[]> GetFileInBuffer(string path)
        {
            await using var fs = new FileStream(path, FileMode.Open);
            var buffer = new byte[fs.Length];
            await fs.ReadAsync(buffer.AsMemory(0, (int)fs.Length));
            fs.Close();
            return buffer;
        }
    }
}


