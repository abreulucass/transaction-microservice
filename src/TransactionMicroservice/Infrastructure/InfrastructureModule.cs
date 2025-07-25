using MongoDB.Driver;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
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
        services.AddMessageService(configuration);
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
            var connectionString = configuration.GetConnectionString("MongoDb");
            var client =  new MongoClient(connectionString);
            
            return client;
        });

        return services;
    }

    private static IServiceCollection AddMessageService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureBusServiceSettings>(
            configuration.GetSection("AzureServiceBus"));
        
        services.AddSingleton<ServiceBusClient>(sp =>
        {
            var connectionString = configuration.GetConnectionString("AzureBusConnection");
            return new ServiceBusClient(connectionString);
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