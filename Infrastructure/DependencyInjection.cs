using System.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDbConnection>(db => new MySqlConnection(
            Environment.GetEnvironmentVariable("CONNECTION_STRING")));
        services.AddScoped<MachineRepository>();
    }
}