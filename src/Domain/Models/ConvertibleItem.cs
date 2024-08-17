using DocConverter.Domain.Attributes.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DocConverter.Domain.Entities
{
    public class ConvertibleItem
    {

        public ConvertibleItem(IFormFile file, string convertTo = "pdf")
        {
            File = file;
            ConvertTo = convertTo;
        }

        [Required]
        public IFormFile File { get; set; }

        [ConvertType]
        public string ConvertTo { get; set; }

        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(File.FileName);
        public string FileExtension => Path.GetExtension(File.FileName);
        public string UniqueFileNameWithoutExtension => $"{FileNameWithoutExtension}_{Guid.NewGuid()}";
        public string UniqueFileName => $"{UniqueFileNameWithoutExtension}{FileExtension}";

    }
}
