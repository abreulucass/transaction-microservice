using Microsoft.Extensions.DependencyInjection;
using TransactionMicroservice.Domain.Interfaces;
using TransactionMicroservice.Infrastructure.Messaging;
using TransactionMicroservice.Infrastructure.Repositories;

namespace TransactionMicroservice.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionRepository, DbTransacationRepository>();
        services.AddSingleton<ITransactionQueueService, FakeServiceBusClient>();
        return services;
    } 
}