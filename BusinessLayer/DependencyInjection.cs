
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(
        this IServiceCollection services)
    {
        services.AddMemoryCache();

        // services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}
