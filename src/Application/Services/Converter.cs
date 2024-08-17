using DocConverter.Application.Interfaces;
using DocConverter.Application.Settings;
using DocConverter.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DocConverter.Application.Services
{
    public class Converter(
        IConverterProvider converter) : IConverter
    {

        public async Task<byte[]> ConvertAsync(IFormFile formFile, string convertTo)
        {

            var uniqueFileName = Path.GetFileNameWithoutExtension(formFile.FileName) + Guid.NewGuid().ToString();
            var fullFileName = uniqueFileName + Path.GetExtension(formFile.FileName);

            await FileHelpers.SaveFileAsync(formFile, fullFileName, AppSettings.WorkingDirectory);
            var outPath = Path.Combine(AppSettings.WorkingDirectory, $"{uniqueFileName}.{convertTo}");

            await converter.ExecuteAsync(fullFileName, $" --headless --convert-to {outPath}");

            var result = await FileHelpers.GetFileInBuffer(outPath);
            ClearFiles(Path.Combine(AppSettings.WorkingDirectory, fullFileName), outPath);

            return result;
        }
        
        private static void ClearFiles(string inFile, string outFile)
        {
            File.Delete(inFile);
            File.Delete(outFile);
        }
    }
}
