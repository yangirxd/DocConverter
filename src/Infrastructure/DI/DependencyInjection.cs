using DocConverter.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DocConverter.Infrastructure.Services;

namespace DocConverter.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IConverterProvider, ConverterProvider>();

            return services;
        }
    }
}
