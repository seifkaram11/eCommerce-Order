using Core.Service;
using Core.ServiceContrast;
using Core.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg=>{},typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssemblyContaining<OrderUpdateRequestValidator>();
        services.AddScoped<IOrdersService, OrdersService>();
        return services;
    }
}
