using DocConverter.Application.Interfaces;
using DocConverter.Application.Settings;
using DocConverter.Application.Helpers;
using DocConverter.Domain.Entities;

namespace DocConverter.Application.Services
{
    public class Converter : IConverter
    {
        private readonly ConvertibleItem _convertibleItem;
        private readonly IConverterProvider _converter;

        public Converter (ConvertibleItem convertibleItem, IConverterProvider converter)
        {
            _convertibleItem = convertibleItem;
            _converter = converter;
        }

        public virtual async Task<byte[]> Convert()
        {
            await FileHelpers.SaveFileAsync(_convertibleItem.File, _convertibleItem.UniqueFileName, AppSettings.WorkingDirectory);
            var outPath = Path.Combine(AppSettings.WorkingDirectory, $"{_convertibleItem.UniqueFileNameWithoutExtension}.{_convertibleItem.ConvertTo}");
            var result = await FileHelpers.GetFileInBuffer(outPath);
            await _converter.Execute(outPath);
            ClearFiles(Path.Combine(AppSettings.WorkingDirectory, _convertibleItem.UniqueFileName), outPath);

            return result;
        }

        private static void ClearFiles(string inFile, string outFile)
        {
            File.Delete(inFile);
            File.Delete(outFile);
        }
    }
}
