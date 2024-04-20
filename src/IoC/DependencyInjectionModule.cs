using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class DependencyInjectionModule
{
    public static void Register(this IServiceCollection services)
    {

    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(e => e.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
    }
}
