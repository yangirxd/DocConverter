namespace DocConverter.Application.Interfaces
{
    public interface IConverter
    {
        Task<byte[]> Convert();
    }
}
