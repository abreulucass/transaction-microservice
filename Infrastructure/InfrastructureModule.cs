using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using TransactionMicroservice.Domain.Interfaces;
using TransactionMicroservice.Infrastructure.Configurations;
using TransactionMicroservice.Infrastructure.Messaging;
using TransactionMicroservice.Infrastructure.Repositories;

namespace TransactionMicroservice.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddMessaging();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var connString = configuration.GetConnectionString("MongoDb");
            var client =  new MongoClient(connString);
            
            return client;
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionRepository, DbTransacationRepository>();
        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionQueueService, MessageSenderService>();
        return services;
    }
}