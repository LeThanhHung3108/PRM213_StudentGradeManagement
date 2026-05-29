
using BusinessLayer.IService;
using BusinessLayer.Service;
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

        services.AddScoped<IImportService, ImportService>();
        services.AddScoped<ISubjectClassService, SubjectClassService>();
        services.AddScoped<IStatisticsService, StatisticsService>();

        return services;
    }
}
