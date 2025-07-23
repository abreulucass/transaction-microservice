using TransactionMicroservice.Application.Services;

namespace TransactionMicroservice.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TransactionService>();
        return services;
    }
}