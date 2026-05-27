using BusinessLayer.Formatters;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<IStatisticsService, StatisticsService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IFgFileService, FgFileService>();

        // File formatters — each format gets its own singleton (they are stateless)
        services.AddSingleton<IFgFileFormatter, CsvFgFileFormatter>();
        services.AddSingleton<IFgFileFormatter, TsvFgFileFormatter>();
        services.AddSingleton<IFgFileFormatter, FixedWidthFgFileFormatter>();

        // IMemoryCache is needed by FgFileService for download-token caching
        services.AddMemoryCache();

        return services;
    }
}
