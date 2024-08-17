using DocConverter.Application.Interfaces;
using DocConverter.Application.Settings;
using DocConverter.Infrastructure.Utilities;

namespace DocConverter.Infrastructure.Services
{
    public class ConverterProvider : IConverterProvider
    {


        public async Task ExecuteAsync(string filePath, string command) 
        {
            await ProcessRunner.RunProcessAsync(
                AppSettings.AppConverter,
                $"{command} \"{filePath}\"",
                AppSettings.WorkingDirectory);
        }
    }
}
