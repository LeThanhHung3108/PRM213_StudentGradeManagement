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
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("MyDB")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IImportedGradeRecordRepository, ImportedGradeRecordRepository>();
        services.AddScoped<IImportHistoryRepository, ImportHistoryRepository>();

        return services;
    }
}
