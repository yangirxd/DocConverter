using DocConverter.Application.Interfaces;
using DocConverter.Application.Settings;
using DocConverter.Domain.Attributes.Validations;
using DocConverter.Domain.Entities;
using DocConverter.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DocConverter.Infrastructure.Services
{
    public class ConverterProvider : IConverterProvider
    {
        public ConverterProvider (IFormFile file, string convertTo = "pdf")
        {
            File = file;
            ConvertTo = convertTo;
        }

        [Required]
        public IFormFile File { get; set; }
        public virtual Guid Guid { get; protected set; } = Guid.NewGuid();

        [ConvertType]
        public string ConvertTo { get; set; }

        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(File.FileName);
        public string FileExtension => Path.GetExtension(File.FileName);
        public string UniqueFileNameWithoutExtension => $"{FileNameWithoutExtension}_{Guid}";
        public string UniqueFileName => $"{UniqueFileNameWithoutExtension}{FileExtension}";

        public async Task Execute(string command) {
            await ProcessRunner.RunProcessAsync(
            AppSettings.AppConverter,
            $"{command} \"{command.UniqueFileName}\"",
            AppSettings.WorkingDirectory);
        }
    }
}
