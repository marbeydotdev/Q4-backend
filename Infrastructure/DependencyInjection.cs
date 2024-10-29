using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<Q4DbContext>(opts =>
        {
            opts.UseMySql("", new MySqlServerVersion(new Version(5, 7)));
        });
    }
}