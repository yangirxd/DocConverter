using DocConverter.Domain.Entities;

namespace DocConverter.Application.Interfaces
{
    public interface IConverterProvider
    {
        Task Execute(string command);
    }
}
