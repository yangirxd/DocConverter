using Microsoft.AspNetCore.Mvc;
using DocConverter.Application.Interfaces;

namespace DocConverter.ConverterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController(
        ILogger<ConverterController> logger,
        IConverter converter) : ControllerBase
    {

        [HttpPost]  
        public async Task<IActionResult> ConvertFile(IFormFile file, string convertTo = "pdf")
        {
            var convertedFile = await converter.ConvertAsync(file, convertTo);
            return File(convertedFile, $"application/{convertTo}", $"{Path.GetFileNameWithoutExtension(file.FileName)}.{convertTo}" );
        }
    }
}
