
using System.ComponentModel.DataAnnotations;

namespace DocConverter.Domain.Attributes.Validations
{
    public class ConvertTypeAttribute : ValidationAttribute
    {
        private readonly string[] _convertTypes = new [] { "pdf", "html" };

        public override bool IsValid(object? value)
        {
            if (value is string type)
            {
                if (_convertTypes.Contains(type))
                    return true;
                else
                    ErrorMessage = $"Incorrect conversion type: {type}";
            }
            return false;
        }
    }
}
