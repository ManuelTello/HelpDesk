using HelpDesk.Service.Abstractions;
using HelpDesk.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
