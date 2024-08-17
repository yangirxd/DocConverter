using DocConverter.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DocConverter.Application.Services;

namespace DocConverter.Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddScoped<IConverter, Converter>();
            return services;
        }
    }
}
