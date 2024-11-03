namespace api.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddMongoDb(
                    mongodbConnectionString: "mongodb://mongodb:27017",
                    name: "mongodb",
                    failureStatus: HealthStatus.Unhealthy
                );

            return services;
        }
    }
}
