
namespace DocConverter.Application.Interfaces
{
    public interface IConverterProvider
    {
        Task ExecuteAsync(string filePath, string command);
    }
}
