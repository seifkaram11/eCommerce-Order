using Core.RedisContrast;
using Core.RepositoryContrast;
using Data;
using Data.Config;
using Infrastructure.Cache;
using Infrastructure.Redis.Data;
using Infrastructure.Repository;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MongoConfigurations.RegisterAll();

        services.AddSingleton<IMongoClient>(_ =>
        {
            var settings = _.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(_ =>
        {
            var clien=_.GetRequiredService<IMongoClient>();
            return clien.GetDatabase(configuration.GetValue<string>("MongoDb:DbName"));
        });

        services.AddScoped<OrderDbContext>();

        services.AddScoped<IOrdersRepository, OrdersRepository>();

        services.AddMassTransit( _ =>
        {
            _.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("order", includeNamespace: false));
            _.AddConsumers(typeof(OrderDbContext).Assembly);

            _.AddMongoDbOutbox(x =>
            {
                x.ClientFactory(_=>_.GetService<IMongoClient>()!);
                x.DatabaseFactory(_=>_.GetService<IMongoDatabase>()!);
                x.UseBusOutbox();
            });

            //to add inboxstate table
            _.AddConfigureEndpointsCallback((context, name, cfg) => cfg.UseMongoDbOutbox(context));

            _.UsingRabbitMq((ctx, cfg) =>
            {
                var rmq = configuration.GetSection("RabbitMq");

                cfg.Host(rmq["Host"], "/", h =>
                {
                    h.Username(rmq["Username"]!);
                    h.Password(rmq["Password"]!);
                });

                cfg.UseMessageRetry(_ => _.Interval(3, TimeSpan.FromMilliseconds(500)));
                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<Client>();

        services.AddScoped<IProductInventoryCache, ProductInventoryCache>();
        return services;
    }
}
