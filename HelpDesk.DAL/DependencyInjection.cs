using HelpDesk.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("SqlConnection") ?? throw new NullReferenceException("connection string not found");
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
}
