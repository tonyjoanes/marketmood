namespace api.Extensions
{
    using api.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("ProductReviewDatabaseSettings")
                             .Get<ProductReviewDatabaseSettings>();

            services.AddHealthChecks()
                .AddMongoDb(
                    mongodbConnectionString: settings.ConnectionString,
                    name: "mongodb",
                    failureStatus: HealthStatus.Unhealthy
                );

            return services;
        }
    }
}
