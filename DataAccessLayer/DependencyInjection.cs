
using DataAccessLayer.Context;
using DataAccessLayer.IRepository;
using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("MyDB")));

        services.AddScoped<IImportRepository, ImportRepository>();
        services.AddScoped<ISubjectClassRepository, SubjectClassRepository>();
        services.AddScoped<IStatisticsRepository, StatisticsRepository>();

        return services;
    }
}