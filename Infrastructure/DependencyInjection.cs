using Core.RepositoryContrast;
using Core.ServiceContrast;
using Data;
using Data.Config;
using FluentValidation;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        MongoConfigurations.RegisterAll();
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });
        services.AddSingleton<MongoDbContext>();
        services.AddSingleton<MongoDbContext>();

        services.AddScoped<IOrdersRepository,OrdersRepository>();
        return services;
    }
}
