using Microsoft.AspNetCore.Http;

namespace DocConverter.Application.Interfaces
{
    public interface IConverter
    {
        Task<byte[]> ConvertAsync(IFormFile formFile, string convertTo);
    }
}
